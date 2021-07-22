using COMP3000Project.Interfaces;
using COMP3000Project.TestObjects;
using COMP3000Project.ViewModel;
using COMP3000Project.Views.Settings;
using COMP3000Project.WS;
using COMP3000Project.Views.Search;
using COMP3000Project.UserDetailsSingleton;
using COMP3000Project.Views.MatchDetails;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using MLToolkit.Forms.SwipeCardView.Core;
using System.Collections.ObjectModel;
using System.Text.Json;
using COMP3000Project.Views.EateryOptionDetails;
using COMP3000Project.Views.FavouriteEateryDetails;

namespace COMP3000Project.Views.Search
{
    class SearchPageViewModel : ViewModelBase, Subscriber
    {
        //VARIABLES
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    SetProperty(ref _username, value);//informs view of change
                }
            }
        }

        ObservableCollection<EateryOption> _eateryOptions;
        public ObservableCollection<EateryOption> EateryOptions { get => _eateryOptions; set => SetProperty(ref _eateryOptions, value); }


        //COMMANDS
        public ICommand Swipe { get; }
        public ICommand GoToEateryOptionDetailsPage { get; }

        //CONSTRUCTOR
        public SearchPageViewModel(string location, string time, string[] eateryTypes)//starting new search
        {
            //set commands
            Swipe = new Command<SwipedCardEventArgs>(ExecuteSwipe);

            //set vars

            EateryOptions = new ObservableCollection<EateryOption>();



            //subscribe to messages
            WebsocketHandler.registerSubscriber(this);

            StartNewSearch(location, time, eateryTypes);
        }

        //CONSTRUCTOR 2
        public SearchPageViewModel(string searchCode)//joining existing search
        {
            //set commands
            Swipe = new Command<SwipedCardEventArgs>(ExecuteSwipe);

            //set vars
            EateryOptions = new ObservableCollection<EateryOption>();

            //subscribe to messages
            WebsocketHandler.registerSubscriber(this);

            JoinExistingSearch(searchCode);
        }

        public async void StartNewSearch(string location, string time, string[] eateryTypes)
        {
            WebsocketHandler.RequestStartNewSearch(UserDetails.Username, UserDetails.Password, location, time, eateryTypes);
        }

        public async void JoinExistingSearch(string searchCode)
        {
            WebsocketHandler.RequestJoinExistingSearch(UserDetails.Username, UserDetails.Password, searchCode);
        }

        async void ExecuteSwipe(SwipedCardEventArgs eventArgs)
        {
            var item = eventArgs.Item as EateryOption;

            switch (eventArgs.Direction.ToString())
            {
                case "Right":
                    WebsocketHandler.RequestCastVote(UserDetails.Username, UserDetails.Password, UserDetails.SearchID, item.ID);
                    break;

                case "Left":

                    break;

                case "Up":
                    EateryOptionDetailsPage nextPage = new EateryOptionDetailsPage(item);

                    await Navigation.PushAsync(nextPage, true);
                    break;

                case "Down":


                    

                    break;

                default:

                    break;
            }
        }






        async void populateOptionsArray(string optionsJSON)
        {
            EateryOptions = JsonSerializer.Deserialize<ObservableCollection<EateryOption>>(optionsJSON);
        }


        public async Task<Location> getLocation()
        {
            Location location;
            try
            {
                location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                    return location;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Console.WriteLine(fnsEx);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Console.WriteLine(fneEx);
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Console.WriteLine(pEx);
            }
            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine(ex);
            }

            Console.WriteLine("LOCATION NOT FOUND");
            return null;
        }

        public void Update(Message message)
        {
            switch (message.type)
            {
                case "newActiveSearchRequestGranted":
                    Console.WriteLine("[MSG] got CREATE active search request granted!");
                    Console.WriteLine("Search ID: " + message.Items[0]);
                    UserDetails.SearchID = message.Items[0].ToString();
                    Console.WriteLine("[MSG] populating array!");
                    populateOptionsArray(message.Items[1].ToString());
                    break;

                case "joinSearchRequestGranted":
                    Console.WriteLine("[MSG] got JOIN active search request granted!");
                    Console.WriteLine("Search ID: " + message.Items[0]);
                    UserDetails.SearchID = message.Items[0].ToString();
                    Console.WriteLine("[MSG] populating array!");
                    populateOptionsArray(message.Items[1].ToString());
                    break;

                case "gotMatch":
                    Console.WriteLine("[MSG] got match!");
                    //navigating to match page
                    EateryOption eateryOption = JsonSerializer.Deserialize<EateryOption>(message.Items[0].ToString());

                    MatchDetailsPage nextPage = new MatchDetailsPage(eateryOption);
                    Navigation.PushAsync(nextPage, true);
                    break;

                default:
                    Console.WriteLine("[MSG] Search page recieved unknown message");
                    break;
            }
        }
    }
}
