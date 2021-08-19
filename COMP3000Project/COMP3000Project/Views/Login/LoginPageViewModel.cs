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
using Plugin.LocalNotification;
using COMP3000Project.LDH;

namespace COMP3000Project.Views.Login
{
    public class LoginPageViewModel : ViewModelBase, Subscriber
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

        private string feedbackText;
        public string FeedbackText
        {
            get { return feedbackText; }
            set
            {
                if (feedbackText != value)
                {
                    SetProperty(ref feedbackText, value);//informs view of change
                }
            }
        }

        private string feedbackTextColour;
        public string FeedbackTextColour
        {
            get { return feedbackTextColour; }
            set
            {
                if (feedbackTextColour != value)
                {
                    SetProperty(ref feedbackTextColour, value);//informs view of change
                }
            }
        }

        private bool feedbackTextIsVisible;
        public bool FeedbackTextIsVisible
        {
            get { return feedbackTextIsVisible; }
            set
            {
                if (feedbackTextIsVisible != value)
                {
                    SetProperty(ref feedbackTextIsVisible, value);//informs view of change
                }
            }
        }

        //COMMANDS
        public ICommand Login { get; }
        public ICommand GoToSignUpPage { get; }

        //CONSTRUCTOR
        public LoginPageViewModel()
        {
            Username = UserDetails.Username;
            Password = UserDetails.Password;

            //set commands
            Login = new Command(async () => await ExecuteLogin());
            GoToSignUpPage = new Command(async () => await ExecuteGoToSignUpPage());

            //set text size
            VeryLarge = UserDetails.GetVeryLargeTextSetting();
            Large = UserDetails.GetlargeTextSetting();
            Medium = UserDetails.GetMediumTextSetting();
            Small = UserDetails.GetSmallTextSetting();

            //set feedback text...
            FeedbackText = "";
            FeedbackTextColour = "Green";
            FeedbackTextIsVisible = false;

            //initialise the connection with the server
            WebsocketHandler.InitialiseConnectionAsync();

        }



        //FUNCTIONS

        //if username and password are null or empty, return false
        public bool UandPAreValid(string username, string password)
        {
            if (username == null || password == null)
            {
                return false;
            }
            else if (username == "" || password == "")
            {
                return false;
            }
            else if (username.Length < 1 || password.Length < 1)
            {
                return false;
            }

            return true;
        }

        //
        async Task<object> ExecuteLogin()
        {
            if(UandPAreValid(Username, Password))
            {
                showFeedBacktext("Logging you in...", "Orange");
                WebsocketHandler.RequestLoginExistingUser(Username, Password);
                return null;
            }


            return null;
        }

        //navigate to sign up page
        async Task ExecuteGoToSignUpPage()
        {
            SignUpPage nextPage = new SignUpPage();
            await Navigation.PushAsync(nextPage, true);
        }

        //hide feedback text
        public void hideLoginFeedbackText()
        {
            FeedbackTextIsVisible = false;
        }

        //show feedback text
        void showFeedBacktext(string message, string colour)
        {
            FeedbackText = message;
            FeedbackTextColour = colour;
            FeedbackTextIsVisible = true;
        }

        public void Update(Message message)
        {
            switch (message.type)
            {
                case "loginRequestGranted":
                    Console.WriteLine("[MSG] login page, proceeding to main menu...");
                    showFeedBacktext("Login successful!", "Green");//show positive feedback

                    UserDetails.setDetails(Username, Password);//set users details
                    LocalDataHandler.SaveUserDetails();//save details locally...

                    MainMenuPage nextPage = new MainMenuPage();//navigate to main menu
                    Navigation.PushAsync(nextPage, true);
                    break;

                case "loginRequestRejected":
                    Console.WriteLine("[MSG] login page, login rejected...");
                    showFeedBacktext("Login rejected", "Red");//show negative feedback
                    break;

                default:
                    Console.WriteLine("[MSG] loginpage recieved unknown message");
                    break;
            }
        }
    }
}
