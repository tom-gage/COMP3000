using COMP3000Project.Interfaces;
using COMP3000Project.TestObjects;
using COMP3000Project.ViewModel;
using COMP3000Project.Views.Settings;
using COMP3000Project.WS;
using COMP3000Project.Views.Search;
using COMP3000Project.Views.SearchParameters;
using COMP3000Project.UserDetailsSingleton;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Text.Json;


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

        //CONSTRUCTOR
        public MainMenuPageViewModel()
        {

            //set commands
            GoToStartSearch = new Command(async () => await GoToExecuteStartSearch());
            GoToJoinSearch = new Command(async () => await GoToExecuteJoinSearch());
            GoToSettings = new Command(async () => await ExecuteGoToSettingsPage());
            GoToStartPastSearch = new Command(async () => await GoToExecuteStartPastSearch());

            WebsocketHandler.registerSubscriber(this);
            //WebsocketHandler.HandleMessages();

            //WebsocketHandler.RequestGetPastSearches(UserDetails.Username, UserDetails.Password);
        }

        //FUNCTIONS
        public bool ValidateSearchCode(string searchCode)
        {
            if (searchCode == null)
            {
                return false;
            } else if (searchCode == "")
            {
                return false;
            } else if (searchCode.Length < 1)
            {
                return false;
            }

            return true;
        }

        async Task<object> GoToExecuteStartPastSearch()
        {
            string[] strArr = { SelectedPastSearch.EateryType };

            SearchPage nextPage = new SearchPage(SelectedPastSearch.Location, SelectedPastSearch.Time, strArr);

            await Navigation.PushAsync(nextPage, true);
            return null;
        }


        async Task<object> GoToExecuteStartSearch()
        {
            //bool startingNewSearch = true;
            //SearchPage nextPage = new SearchPage(startingNewSearch, "");
            SearchParametersPage nextPage = new SearchParametersPage();

            await Navigation.PushAsync(nextPage, true);
            return null;
        }

        async Task<object> GoToExecuteJoinSearch()
        {
            if (ValidateSearchCode(SearchCode))
            {
                bool startingNewSearch = false;
                SearchPage nextPage = new SearchPage(SearchCode);
                await Navigation.PushAsync(nextPage, true);
                return null;
            }
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
            PastSearches = JsonSerializer.Deserialize<ObservableCollection<PastSearch>>(optionsJSON);
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
