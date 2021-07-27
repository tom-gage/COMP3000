using COMP3000Project.Interfaces;
using COMP3000Project.TestObjects;
using COMP3000Project.ViewModel;
using COMP3000Project.Views.Settings;
using COMP3000Project.WS;
using COMP3000Project.Views.Search;
using COMP3000Project.Views.SearchParameters;
using COMP3000Project.UserDetailsSingleton;
using COMP3000Project.Views.FavouriteEateries;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Text.Json;
using COMP3000Project.Views.JoinSearchParameters;

namespace COMP3000Project.Views.MainMenu
{
    public class MainMenuPageViewModel : ViewModelBase, Subscriber
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

        private string _searchCode;
        public string SearchCode
        {
            get { return _searchCode; }
            set
            {
                if (_searchCode != value)
                {
                    SetProperty(ref _searchCode, value);//informs view of change
                }
            }
        }

        private PastSearch _selectedPastSearch;
        public PastSearch SelectedPastSearch
        {
            get { return _selectedPastSearch; }
            set
            {
                if (_selectedPastSearch != value)
                {
                    SetProperty(ref _selectedPastSearch, value);//informs view of change
                }
            }
        }



        ObservableCollection<PastSearch> _pastSearches;
        public ObservableCollection<PastSearch> PastSearches { get => _pastSearches; set => SetProperty(ref _pastSearches, value); }

        //COMMANDS
        public ICommand GoToStartSearch { get; }
        public ICommand GoToJoinSearch { get; }
        public ICommand GoToSettings { get; }
        public ICommand GoToStartPastSearch { get; }
        public ICommand GoToFavourites { get; }

        //CONSTRUCTOR
        public MainMenuPageViewModel()
        {

            //set commands
            GoToStartSearch = new Command(async () => await ExecuteGoToStartSearch());
            GoToJoinSearch = new Command(async () => await ExecuteGoToJoinSearch());
            GoToSettings = new Command(async () => await ExecuteGoToSettingsPage());
            GoToStartPastSearch = new Command(async () => await ExecuteGoToStartPastSearch());
            GoToFavourites = new Command(async () => await GoToFavouritesPage());


            WebsocketHandler.registerSubscriber(this);
            //WebsocketHandler.HandleMessages();

            //WebsocketHandler.RequestGetPastSearches(UserDetails.Username, UserDetails.Password);
        }

        //FUNCTIONS


        async Task<object> GoToFavouritesPage()
        {

            FavouriteEateriesPage nextPage = new FavouriteEateriesPage();

            await Navigation.PushAsync(nextPage, true);
            return null;
        }

        async Task<object> ExecuteGoToStartPastSearch()
        {
            string[] strArr = { SelectedPastSearch.EateryType };

            SearchParametersPage nextPage = new SearchParametersPage(SelectedPastSearch.Location, SelectedPastSearch.Time, strArr);

            await Navigation.PushAsync(nextPage, true);
            return null;
        }


        async Task<object> ExecuteGoToStartSearch()
        {
            SearchParametersPage nextPage = new SearchParametersPage();

            await Navigation.PushAsync(nextPage, true);
            return null;
        }

        async Task<object> ExecuteGoToJoinSearch()
        {

            JoinSearchParametersPage nextPage = new JoinSearchParametersPage();

            await Navigation.PushAsync(nextPage, true);
            return null;

        }

        async Task<object> ExecuteGoToSettingsPage()
        {
            SettingsPage nextPage = new SettingsPage();
            await Navigation.PushAsync(nextPage, true);

            return null;
        }


        async void populatePastSearchesArray(string optionsJSON)
        {
            PastSearches = sortByLatest(JsonSerializer.Deserialize<ObservableCollection<PastSearch>>(optionsJSON));
        }

        ObservableCollection<PastSearch> sortByLatest(ObservableCollection<PastSearch> pastSearches)
        {
            PastSearch[] pastSearchArray = new List<PastSearch>(pastSearches).ToArray();
            pastSearches.Clear();

            PastSearch t;

            for (int p = 0; p <= pastSearchArray.Length - 2; p++)
            {
                for (int i = 0; i <= pastSearchArray.Length - 2; i++)
                {
                    DateTime a = new DateTime(int.Parse(pastSearchArray[i].YearOfSearch), int.Parse(pastSearchArray[i].MonthOfSearch), int.Parse(pastSearchArray[i].DayOfSearch), int.Parse(pastSearchArray[i].Time.Substring(0, 2)), int.Parse(pastSearchArray[i].Time.Substring(2, 2)), 0);
                    DateTime b = new DateTime(int.Parse(pastSearchArray[i + 1].YearOfSearch), int.Parse(pastSearchArray[i + 1].MonthOfSearch), int.Parse(pastSearchArray[i + 1].DayOfSearch), int.Parse(pastSearchArray[i + 1].Time.Substring(0, 2)), int.Parse(pastSearchArray[i].Time.Substring(2, 2)), 0);

                    if (a < b)
                    {
                        t = pastSearchArray[i + 1];
                        pastSearchArray[i + 1] = pastSearchArray[i];
                        pastSearchArray[i] = t;
                    }
                }
            }

            foreach (PastSearch ps in pastSearchArray)
            {
                pastSearches.Add(ps);
            }


            return pastSearches;
        }

        public void Update(Message message)
        {
            switch (message.type)
            {
                case "":
                    Console.WriteLine("...");

                    break;

                case "gotPastSearches":
                    Console.WriteLine("[MSG] GOT PAST SEARCHES!");
                    Console.WriteLine(message.Items[0]);
                    populatePastSearchesArray(message.Items[0].ToString());
                    
                    //now populate past searches array...


                    break;

                default:
                    Console.WriteLine("[MSG] main menu recieved unknown message");
                    break;
            }
        }
    }
}
