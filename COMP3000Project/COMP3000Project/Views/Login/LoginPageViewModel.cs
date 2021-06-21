using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using COMP3000Project.ViewModel;
using COMP3000Project.WS;
using COMP3000Project.Views.SignUp;
using COMP3000Project.Interfaces;
using Newtonsoft.Json;
using COMP3000Project.TestObjects;
using COMP3000Project.Views.MainMenu;
using COMP3000Project.UserDetailsSingleton;

namespace COMP3000Project.Views.Login
{
    class LoginPageViewModel : ViewModelBase, Subscriber
    {
        //VARIABLES
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                if(_username != value)
                {
                    SetProperty(ref _username, value);//informs view of change
                }
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    SetProperty(ref _password, value);//informs view of change
                }
            }
        }

        //COMMANDS
        public ICommand Login { get; }
        public ICommand GoToSignUpPage { get; }

        //CONSTRUCTOR
        public LoginPageViewModel()
        {
            Username = "u";//hardcoded for now, V TEMPORARY :<<<<<
            Password = "p";

            //set commands
            Login = new Command(async () => await ExecuteLogin());
            GoToSignUpPage = new Command(async () => await ExecuteGoToSignUpPage());



            WebsocketHandler.InitialiseConnectionAsync();
            WebsocketHandler.registerSubscriber(this);
        }

        //FUNCTIONS
        async Task<object> ExecuteLogin()
        {
            UserDetails.setDetails(Username, Password);

            WebsocketHandler.RequestLoginExistingUser(Username, Password);

            return null;
        }

        async Task ExecuteGoToSignUpPage()
        {
            SignUpPage nextPage = new SignUpPage();
            await Navigation.PushAsync(nextPage, true);
        }

        public void Update(Message message)
        {
            switch (message.type)
            {
                case "loginRequestGranted":
                    Console.WriteLine("[MSG] login page, proceeding to main menu...");

                    UserDetails.setDetails(Username, Password);

                    MainMenuPage nextPage = new MainMenuPage();
                    Navigation.PushAsync(nextPage, true);
                    break;

                default:
                    Console.WriteLine("[MSG] loginpage recieved unknown message");
                    break;
            }
        }
    }
}
