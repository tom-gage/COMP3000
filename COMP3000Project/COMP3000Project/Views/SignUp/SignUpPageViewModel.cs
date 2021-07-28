using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using COMP3000Project.Interfaces;
using COMP3000Project.TestObjects;
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
            RegisterNewUser = new Command(async () => await ExecuteRegisterUser());

            WebsocketHandler.registerSubscriber(this);
        }

        //FUNCTIONS
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

        async Task<object> ExecuteRegisterUser()
        {
            if(UandPAreValid(Username, Password))
            {
                showRegistrationInProgress();
                WebsocketHandler.RequestRegisterNewUser(Username, Password);

                return null;
            }
            return null;
        }


        public void hideLoginFeedbackText()
        {
            FeedbackTextIsVisible = false;
        }
        void showRegistrationSuccess()
        {
            FeedbackText = "Registration successful!";
            FeedbackTextColour = "Green";
            FeedbackTextIsVisible = true;
        }
        void showRegistrationInProgress()
        {
            FeedbackText = "Processing...";
            FeedbackTextColour = "Orange";
            FeedbackTextIsVisible = true;
        }
        void showRegistrationFailed()
        {
            FeedbackText = "Registration rejected, username is taken!";
            FeedbackTextColour = "Red";
            FeedbackTextIsVisible = true;
        }


        public void Update(Message message)
        {
            switch (message.type)
            {
                case "registrationSuccess":

                    showRegistrationSuccess();
                    
                    Navigation.PopAsync();

                    break;

                case "registrationRequestRejected":

                    showRegistrationFailed();

                    break;

                default:
                    Console.WriteLine("[MSG] sign up page recieved unknown message");
                    break;
            }
        }

    }
}
