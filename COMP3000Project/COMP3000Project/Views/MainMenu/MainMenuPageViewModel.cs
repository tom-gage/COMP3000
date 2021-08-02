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
using COMP3000Project.Views.AccessibilitySettings;

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

        public ICommand GoToAccessibilitySettings { get; }

        //CONSTRUCTOR
        public MainMenuPageViewModel()
        {
            //set commands
            GoToStartSearch = new Command(async () => await ExecuteGoToStartSearch());
            GoToJoinSearch = new Command(async () => await ExecuteGoToJoinSearch());
            GoToSettings = new Command(async () => await ExecuteGoToSettingsPage());
            GoToStartPastSearch = new Command(async () => await ExecuteGoToStartPastSearch());
            GoToFavourites = new Command(async () => await GoToFavouritesPage());
            GoToAccessibilitySettings = new Command(async () => await ExecuteGoToAccessibilitySettings());

            //set text size
            VeryLarge = UserDetails.GetVeryLargeTextSetting();
            Large = UserDetails.GetlargeTextSetting();
            Medium = UserDetails.GetMediumTextSetting();
            Small = UserDetails.GetSmallTextSetting();

            //register this class as a subscriber to the websocket handler, allows for the recieving of inter class messages
            WebsocketHandler.registerSubscriber(this);
        }

        //FUNCTIONS

        //navigate to the favourites page
        async Task<object> GoToFavouritesPage()
        {

            FavouriteEateriesPage nextPage = new FavouriteEateriesPage();

            await Navigation.PushAsync(nextPage, true);
            return null;
        }

        //navigate to pre-search parameters page, use past search constructor
        async Task<object> ExecuteGoToStartPastSearch()
        {
            string[] strArr = { SelectedPastSearch.EateryType };

            SearchParametersPage nextPage = new SearchParametersPage(SelectedPastSearch.Location, SelectedPastSearch.Time, strArr);

            await Navigation.PushAsync(nextPage, true);
            return null;
        }

        //navigate to pre-search parameters page, use start new search constructor
        async Task<object> ExecuteGoToStartSearch()
        {
            SearchParametersPage nextPage = new SearchParametersPage();

            await Navigation.PushAsync(nextPage, true);
            return null;
        }

        //navigate to join search page
        async Task<object> ExecuteGoToJoinSearch()
        {

            JoinSearchParametersPage nextPage = new JoinSearchParametersPage();

            await Navigation.PushAsync(nextPage, true);
            return null;

        }

        //navigate to settings page
        async Task<object> ExecuteGoToSettingsPage()
        {
            SettingsPage nextPage = new SettingsPage();
            await Navigation.PushAsync(nextPage, true);

            return null;
        }

        //navigate to accessibility settings page
        async Task<object> ExecuteGoToAccessibilitySettings()
        {
            AccessibilitySettingsPage nextPage = new AccessibilitySettingsPage();
            await Navigation.PushAsync(nextPage, true);
            return null;
        }

        //populate past searches array
        async void populatePastSearchesArray(string optionsJSON)
        {
            PastSearches = sortByLatest(JsonSerializer.Deserialize<ObservableCollection<PastSearch>>(optionsJSON));
        }

        ObservableCollection<PastSearch> sortByLatest(ObservableCollection<PastSearch> pastSearches)
        {
            //convert collection to array
            PastSearch[] pastSearchArray = new List<PastSearch>(pastSearches).ToArray();
            //clear collection
            pastSearches.Clear();

            PastSearch t;

            for (int p = 0; p <= pastSearchArray.Length - 2; p++)
            {
                for (int i = 0; i <= pastSearchArray.Length - 2; i++)
                {
                    //convert to datetime for comparison
                    DateTime a = new DateTime(int.Parse(pastSearchArray[i].YearOfSearch), int.Parse(pastSearchArray[i].MonthOfSearch), int.Parse(pastSearchArray[i].DayOfSearch), int.Parse(pastSearchArray[i].Time.Substring(0, 2)), int.Parse(pastSearchArray[i].Time.Substring(2, 2)), 0);
                    DateTime b = new DateTime(int.Parse(pastSearchArray[i + 1].YearOfSearch), int.Parse(pastSearchArray[i + 1].MonthOfSearch), int.Parse(pastSearchArray[i + 1].DayOfSearch), int.Parse(pastSearchArray[i + 1].Time.Substring(0, 2)), int.Parse(pastSearchArray[i].Time.Substring(2, 2)), 0);

                    if (a < b)//if selected past search's(a) date is earlier than following neighbour (b), swap past searches
                    {
                        t = pastSearchArray[i + 1];
                        pastSearchArray[i + 1] = pastSearchArray[i];
                        pastSearchArray[i] = t;
                    }
                }
            }

            //fillcollection
            foreach (PastSearch ps in pastSearchArray)
            {
                pastSearches.Add(ps);
            }

            //return sorted collection
            return pastSearches;
        }

        //catches incoming messages from the publisher
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

                    break;

                default:
                    Console.WriteLine("[MSG] main menu recieved unknown message");
                    break;
            }
        }
    }
}
