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
            Username = "u";//hardcoded for now, V TEMPORARY :<<<<<
            Password = "p";

            //set commands
            Login = new Command(async () => await ExecuteLogin());
            GoToSignUpPage = new Command(async () => await ExecuteGoToSignUpPage());

            //set text size
            VeryLarge = UserDetails.GetVeryLargeTextSetting();
            Large = UserDetails.GetlargeTextSetting();
            Medium = UserDetails.GetMediumTextSetting();
            Small = UserDetails.GetSmallTextSetting();

            FeedbackText = "";
            FeedbackTextColour = "Green";
            FeedbackTextIsVisible = false;


            WebsocketHandler.InitialiseConnectionAsync();
            WebsocketHandler.registerSubscriber(this);
        }

        //FUNCTIONS

        void displayNotification()
        {
            var notification = new NotificationRequest
            {
                BadgeNumber = 0,
                Description = "this is my notification",
                Title = "this is the title",
                ReturningData = "this is the returning data",
                NotificationId = 123,
                CategoryType = NotificationCategoryType.Progress
            };

            NotificationCenter.Current.Show(notification);
        }

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

        async Task<object> ExecuteLogin()
        {
            if(UandPAreValid(Username, Password))
            {
                showLoginInProgress();
                WebsocketHandler.RequestLoginExistingUser(Username, Password);
                return null;
            }


            return null;
        }

        async Task ExecuteGoToSignUpPage()
        {
            SignUpPage nextPage = new SignUpPage();
            await Navigation.PushAsync(nextPage, true);
        }

        public void hideLoginFeedbackText()
        {
            FeedbackTextIsVisible = false;
        }
        void showLoginSuccess()
        {
            FeedbackText = "Login successful!";
            FeedbackTextColour = "Green";
            FeedbackTextIsVisible = true;
        }
        void showLoginInProgress()
        {
            FeedbackText = "Processing...";
            FeedbackTextColour = "Orange";
            FeedbackTextIsVisible = true;
        }
        void showLoginRejected()
        {
            FeedbackText = "Login rejected!";
            FeedbackTextColour = "Red";
            FeedbackTextIsVisible = true;
        }


        public void Update(Message message)
        {
            switch (message.type)
            {
                case "loginRequestGranted":
                    Console.WriteLine("[MSG] login page, proceeding to main menu...");
                    showLoginSuccess();

                    UserDetails.setDetails(Username, Password);

                    MainMenuPage nextPage = new MainMenuPage();
                    Navigation.PushAsync(nextPage, true);
                    break;

                case "loginRequestRejected":
                    Console.WriteLine("[MSG] login page, login rejected...");
                    showLoginRejected();

                    break;

                default:
                    Console.WriteLine("[MSG] loginpage recieved unknown message");
                    break;
            }
        }
    }
}
