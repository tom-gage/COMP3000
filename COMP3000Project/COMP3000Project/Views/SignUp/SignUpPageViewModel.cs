using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using COMP3000Project.Interfaces;
using COMP3000Project.LDH;
using COMP3000Project.TestObjects;
using COMP3000Project.UserDetailsSingleton;
using COMP3000Project.ViewModel;
using COMP3000Project.WS;
using Xamarin.Forms;

namespace COMP3000Project.Views.SignUp
{
    public class SignUpPageViewModel : ViewModelBase, Subscriber
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
        public ICommand RegisterNewUser { get; }

        //CONSTRUCTOR
        public SignUpPageViewModel()
        {
            //set command
            RegisterNewUser = new Command(async () => await ExecuteRegisterUser());

            //set text size
            VeryLarge = UserDetails.GetVeryLargeTextSetting();
            Large = UserDetails.GetlargeTextSetting();
            Medium = UserDetails.GetMediumTextSetting();
            Small = UserDetails.GetSmallTextSetting();

        }

        //FUNCTIONS
        //returns false if username or password is null or empty
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

        //shows feedback and asks the WSH to send a message to the server asking to register a new user
        async Task<object> ExecuteRegisterUser()
        {
            if(UandPAreValid(Username, Password))
            {
                showFeedBacktext("Processing...", "Orange");//show feedback
                WebsocketHandler.RequestRegisterNewUser(Username, Password);//send request to server

                return null;
            }
            return null;
        }

        //hises the feedback text
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
                case "registrationSuccess"://registration was successfull
                    showFeedBacktext("Registration Successfull!", "Green");//show success feedback

                    UserDetails.Username = message.Items[0].ToString();//set username & password
                    UserDetails.Password = message.Items[1].ToString();

                    LocalDataHandler.SaveUserDetails();//save details locally

                    Navigation.PopAsync();//return to login page

                    break;

                case "registrationRequestRejected":

                    showFeedBacktext("Registration rejected, username is taken!", "Red");//show failure feedback

                    break;

                default:
                    Console.WriteLine("[MSG] sign up page recieved unknown message");
                    break;
            }
        }

    }
}
