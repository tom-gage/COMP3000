using COMP3000Project.Interfaces;
using COMP3000Project.TestObjects;
using COMP3000Project.ViewModel;
using COMP3000Project.UserDetailsSingleton;
using COMP3000Project.Views.Settings;
using COMP3000Project.WS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace COMP3000Project.Views.Settings
{
    public class SettingsPageViewModel : ViewModelBase, Subscriber
    {
        //VARIABLES
        private string _currentUsername;
        public string CurrentUsername
        {
            get { return _currentUsername; }
            set
            {
                if (_currentUsername != value)
                {
                    SetProperty(ref _currentUsername, value);//informs view of change
                }
            }
        }

        private string _currentPassword;
        public string CurrentPassword
        {
            get { return _currentPassword; }
            set
            {
                if (_currentPassword != value)
                {
                    SetProperty(ref _currentPassword, value);//informs view of change
                }
            }
        }

        private string _newUsername;
        public string NewUsername
        {
            get { return _newUsername; }
            set
            {
                if (_newUsername != value)
                {
                    SetProperty(ref _newUsername, value);//informs view of change
                }
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                if (_newPassword != value)
                {
                    SetProperty(ref _newPassword, value);//informs view of change
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
        public ICommand ChangeUsername { get; }
        public ICommand ChangePassword { get; }
        public ICommand DeleteAccount { get; }

        //CONSTRUCTOR
        public SettingsPageViewModel()
        {
            CurrentUsername = UserDetails.Username;
            CurrentPassword = UserDetails.Password;

            NewUsername = "";
            NewPassword = "";

            //set commands
            ChangeUsername = new Command(async () => await ExecuteChangeUsername());
            ChangePassword = new Command(async () => await ExecuteChangePassword());
            DeleteAccount = new Command(async () => await ExecuteDeleteAccount());

            //set text size
            VeryLarge = UserDetails.GetVeryLargeTextSetting();
            Large = UserDetails.GetlargeTextSetting();
            Medium = UserDetails.GetMediumTextSetting();
            Small = UserDetails.GetSmallTextSetting();

            WebsocketHandler.registerSubscriber(this);//subscribe to messages from the WSH
            //WebsocketHandler.HandleMessages();
        }

        //FUNCTIONS

        public bool UsernameIsValid(string username)
        {
            if (username == null)
            {
                return false;
            }
            else if (username == "")
            {
                return false;
            }
            else if (username.Length < 1)
            {
                return false;
            }

            return true;
        }

        public bool PasswordIsValid(string password)
        {
            if (password == null)
            {
                return false;
            }
            else if (password == "")
            {
                return false;
            }
            else if (password.Length < 1)
            {
                return false;
            }

            return true;
        }

        async Task<object> ExecuteChangeUsername()
        {
            if (UsernameIsValid(NewUsername))
            {
                showFeedbackText("Updating username...", "Orange");
                WebsocketHandler.RequestChangeUsername(CurrentUsername, CurrentPassword, NewUsername);

                return null;
            }

            return null;
        }

        async Task<object> ExecuteChangePassword()
        {
            if (PasswordIsValid(NewPassword))
            {
                showFeedbackText("Updating password...", "Orange");
                WebsocketHandler.RequestChangePassword(CurrentUsername, CurrentPassword, NewPassword);

                return null;
            }


            return null;
        }

        async Task<object> ExecuteDeleteAccount()
        {
            showFeedbackText("Processing...", "Orange");
            WebsocketHandler.RequestDeleteUser(CurrentUsername, CurrentPassword);

            return null;
        }


        public void hideFeedbackText()
        {
            FeedbackTextIsVisible = false;
        }

        void showFeedbackText(string message, string colour)
        {
            FeedbackText = message;
            FeedbackTextColour = colour;
            FeedbackTextIsVisible = true;
        }


        public void Update(Message message)
        {
            switch (message.type)
            {
                case "usernameUpdated":
                    Console.WriteLine("[MSG] Settings page, username update");
                    showFeedbackText("Username updated!", "Green");
                    UserDetails.Username = message.Body;
                    CurrentUsername = message.Body;


                    break;

                case "usernameChangeRequestRejected":
                    Console.WriteLine("[MSG] Settings page, username update rejected");
                    showFeedbackText("Username update failed, username is taken!", "Red");


                    break;


                case "passwordUpdated":
                    Console.WriteLine("[MSG] Settings page, password update");
                    showFeedbackText("Password updated!", "Green");
                    UserDetails.Password = message.Body;
                    CurrentPassword = message.Body;
                    break;


                case "passwordUpdateRequestRejected":
                    Console.WriteLine("[MSG] Settings page, username update rejected");
                    showFeedbackText("Password update failed!", "Red");


                    break;

                case "userDeleted":
                    Console.WriteLine("[MSG] Settings page, user delete");
                    showFeedbackText("Account deleted!", "Red");
                    UserDetails.clearDetails();


                    Navigation.PopToRootAsync();
                    break;

                default:
                    Console.WriteLine("[MSG] Settings page recieved unknown message");
                    break;
            }
        }
    }
}
