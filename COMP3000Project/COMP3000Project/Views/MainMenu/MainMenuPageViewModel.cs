using COMP3000Project.Interfaces;
using COMP3000Project.TestObjects;
using COMP3000Project.ViewModel;
using COMP3000Project.Views.Settings;
using COMP3000Project.WS;
using COMP3000Project.Views.Search;
using COMP3000Project.Views.SearchParameters;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

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

        //COMMANDS
        public ICommand GoToStartSearch { get; }
        public ICommand GoToJoinSearch { get; }
        public ICommand GoToSettings { get; }

        //CONSTRUCTOR
        public MainMenuPageViewModel()
        {

            //set commands
            GoToStartSearch = new Command(async () => await GoToExecuteStartSearch());
            GoToJoinSearch = new Command(async () => await GoToExecuteJoinSearch());
            GoToSettings = new Command(async () => await ExecuteGoToSettingsPage());

            WebsocketHandler.registerSubscriber(this);
            //WebsocketHandler.HandleMessages();
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

        public void Update(Message message)
        {
            switch (message.type)
            {
                case "":
                    Console.WriteLine("...");

                    break;

                default:
                    Console.WriteLine("[MSG] main menu recieved unknown message");
                    break;
            }
        }
    }
}
