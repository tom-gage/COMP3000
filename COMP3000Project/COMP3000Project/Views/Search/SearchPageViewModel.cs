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
using COMP3000Project.Views.Tutorial;
using COMP3000Project.LDH;

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


        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    SetProperty(ref title, value);//informs view of change
                }
            }
        }

        private string searchCode;
        public string SearchCode
        {
            get { return "Your search code is: " + searchCode; }
            set
            {
                if (searchCode != value)
                {
                    SetProperty(ref searchCode, value);//informs view of change
                }
            }
        }


        private uint _threshold;
        public uint Threshold
        {
            get => _threshold;
            set
            {
                _threshold = value;
                SetProperty(ref _threshold, value);//informs view of change
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
            Threshold = (uint)(App.ScreenWidth / 3);
            EateryOptions = new ObservableCollection<EateryOption>();


            SearchCode = "pending...";

            //set text size
            VeryLarge = UserDetails.GetVeryLargeTextSetting();
            Large = UserDetails.GetlargeTextSetting();
            Medium = UserDetails.GetMediumTextSetting();
            Small = UserDetails.GetSmallTextSetting();


            //populate eateryOptions array
            StartNewSearch(location, time, eateryTypes);

            //register this class as a subscriber to the websocket handler, allows for the recieving of inter class messages
            WebsocketHandler.registerSubscriber(this);
        }

        //CONSTRUCTOR 2
        public SearchPageViewModel(string eateryOptionsJson)//joining existing search
        {
            //set commands
            Swipe = new Command<SwipedCardEventArgs>(ExecuteSwipe);

            //set vars
            Threshold = (uint)(App.ScreenWidth / 3);
            EateryOptions = new ObservableCollection<EateryOption>();

            SearchCode = "pending...";

            //set text size
            VeryLarge = UserDetails.GetVeryLargeTextSetting();
            Large = UserDetails.GetlargeTextSetting();
            Medium = UserDetails.GetMediumTextSetting();
            Small = UserDetails.GetSmallTextSetting();

            //populate eateryOptions array
            populateOptionsArray(eateryOptionsJson);

            //register this class as a subscriber to the websocket handler, allows for the recieving of inter class messages
            WebsocketHandler.registerSubscriber(this);
        }

        //checks for first start up, opens tutorial page if true
        public async void OpenTutorialPage()
        {
            if (UserDetails.SearchPageTutorialShown == false)
            {
                UserDetails.SearchPageTutorialShown = true;
                await LocalDataHandler.SaveUserDetails();

                TutorialPage nextPage = new TutorialPage();
                await Navigation.PushAsync(nextPage, true);
            }
        }


        //starts a new search
        public async void StartNewSearch(string location, string time, string[] eateryTypes)
        {
            WebsocketHandler.RequestStartNewSearch(UserDetails.Username, UserDetails.Password, location, time, eateryTypes);
        }

        //join an existing search 
        public async void JoinExistingSearch(string searchCode)
        {
            WebsocketHandler.RequestJoinExistingSearch(UserDetails.Username, UserDetails.Password, searchCode);
        }

        //on card swiped...
        async void ExecuteSwipe(SwipedCardEventArgs eventArgs)
        {
            var item = eventArgs.Item as EateryOption;

            

            switch (eventArgs.Direction.ToString())//get swipe direction
            {
                case "Right"://if right, cast vote for option
                    WebsocketHandler.RequestCastVote(UserDetails.Username, UserDetails.Password, UserDetails.SearchID, item.ID);
                    break;

                case "Left":

                    break;

                case "Up"://if up, navigate to details page


                    EateryOptionDetailsPage nextPage = new EateryOptionDetailsPage(item);

                    await Navigation.PushAsync(nextPage, true);
                    break;

                case "Down":


                    

                    break;

                default:

                    break;
            }
        }





        //populate options array
        async void populateOptionsArray(string optionsJSON)
        {
            EateryOptions = JsonSerializer.Deserialize<ObservableCollection<EateryOption>>(optionsJSON);
        }


        //public async Task<Location> getLocation()
        //{
        //    Location location;
        //    try
        //    {
        //        location = await Geolocation.GetLastKnownLocationAsync();

        //        if (location != null)
        //        {
        //            Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

        //            return location;
        //        }
        //    }
        //    catch (FeatureNotSupportedException fnsEx)
        //    {
        //        // Handle not supported on device exception
        //        Console.WriteLine(fnsEx);
        //    }
        //    catch (FeatureNotEnabledException fneEx)
        //    {
        //        // Handle not enabled on device exception
        //        Console.WriteLine(fneEx);
        //    }
        //    catch (PermissionException pEx)
        //    {
        //        // Handle permission exception
        //        Console.WriteLine(pEx);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Unable to get location
        //        Console.WriteLine(ex);
        //    }

        //    Console.WriteLine("LOCATION NOT FOUND");
        //    return null;
        //}

        //catches incoming messages from the publisher
        public void Update(Message message)
        {
            switch (message.type)
            {
                case "newActiveSearchRequestGranted":
                    Console.WriteLine("[MSG] got CREATE active search request granted!");
                    Console.WriteLine("Search ID: " + message.Items[0]);
                    UserDetails.SearchID = message.Items[0].ToString();
                    SearchCode = message.Items[0].ToString();
                    Console.WriteLine("[MSG] populating array!");
                    populateOptionsArray(message.Items[1].ToString());
                    break;

                case "joinSearchRequestGranted":
                    Console.WriteLine("[MSG] got JOIN active search request granted!");
                    Console.WriteLine("Search ID: " + message.Items[0]);
                    UserDetails.SearchID = message.Items[0].ToString();
                    SearchCode = message.Items[0].ToString();

                    Console.WriteLine("[MSG] populating array!");
                    populateOptionsArray(message.Items[1].ToString());
                    break;

                case "participantVoted":
                    Console.WriteLine("[MSG] a participant voted");
                    ShowToast(message.Body + " Voted!");
                    break;

                case "participantJoined":
                    Console.WriteLine("[MSG] a participant joined");
                    ShowToast(message.Body + " joined!");
                    break;

                case "matched!":
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
