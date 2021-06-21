using COMP3000Project.Interfaces;
using COMP3000Project.TestObjects;
using COMP3000Project.ViewModel;
using COMP3000Project.Views.Settings;
using COMP3000Project.WS;
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
        public ICommand StartSearch { get; }
        public ICommand JoinSearch { get; }
        public ICommand GoToSettings { get; }

        //CONSTRUCTOR
        public MainMenuPageViewModel()
        {

            //set commands
            StartSearch = new Command(async () => await ExecuteStartSearch());
            JoinSearch = new Command(async () => await ExecuteJoinSearch());
            GoToSettings = new Command(async () => await ExecuteGoToSettingsPage());

            WebsocketHandler.registerSubscriber(this);
            //WebsocketHandler.HandleMessages();
        }

        //FUNCTIONS
        async Task<object> ExecuteStartSearch()
        {
            //WebsocketHandler.RequestStartNewSearch();

            return null;
        }

        async Task<object> ExecuteJoinSearch()
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
