using COMP3000Project.Interfaces;
using COMP3000Project.TestObjects;
using COMP3000Project.ViewModel;
using COMP3000Project.Views.Settings;
using COMP3000Project.WS;
using COMP3000Project.Views.Search;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace COMP3000Project.Views.MainMenu
{
    class MainMenuPageViewModel : ViewModelBase, Subscriber
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
        async Task<object> GoToExecuteStartSearch()
        {
            //WebsocketHandler.RequestStartNewSearch();
            SearchPage nextPage = new SearchPage(true, "");
            await Navigation.PushAsync(nextPage, true);
            return null;
        }

        async Task<object> GoToExecuteJoinSearch()
        {
            //WebsocketHandler.RequestJoinExistingSearch();

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
