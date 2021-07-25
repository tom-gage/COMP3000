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
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using COMP3000Project.UserDetailsSingleton;

namespace COMP3000Project.Views.JoinSearchParameters
{
    class JoinSearchParametersPageViewModel : ViewModelBase, Subscriber
    {
        //PROPERTIES
        private string searchCode;
        public string SearchCode
        {
            get { return searchCode; }
            set
            {
                if (searchCode != value)
                {
                    SetProperty(ref searchCode, value);//informs view of change
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

        public ICommand JoinSearch { get; }

        //CONSTRUCTOR
        public JoinSearchParametersPageViewModel()
        {
            //set commands
            JoinSearch = new Command(async () => await ExecuteJoinSearch());


            WebsocketHandler.registerSubscriber(this);
        }


        //FUNCTIONS
        async Task<object> ExecuteJoinSearch()
        {
            if (ValidateSearchCode(SearchCode))
            {
                showJoinInProgress();
                WebsocketHandler.RequestJoinExistingSearch(UserDetails.Username, UserDetails.Password, SearchCode);

            } else
            {
                showJoinRejected("Please enter a join code!");
            }
            return null;
        }

        public void hideFeedbackText()
        {
            FeedbackTextIsVisible = false;
        }

        void showJoinSuccess()
        {
            FeedbackText = "Join search successful!";
            FeedbackTextColour = "Green";
            FeedbackTextIsVisible = true;
        }
        void showJoinInProgress()
        {
            FeedbackText = "Joining search...";
            FeedbackTextColour = "Orange";
            FeedbackTextIsVisible = true;
        }
        void showJoinRejected(string rejectionText)
        {
            FeedbackText = rejectionText;
            FeedbackTextColour = "Red";
            FeedbackTextIsVisible = true;
        }


        public bool ValidateSearchCode(string searchCode)
        {
            if (searchCode == null)
            {
                return false;
            }
            else if (searchCode == "")
            {
                return false;
            }
            else if (searchCode.Length < 1)
            {
                return false;
            }

            return true;
        }

        async public void Update(Message message)
        {
            switch (message.type)
            {
                case "":
                    Console.WriteLine("...");

                    break;

                case "joinSearchRequestGranted":
                    Console.WriteLine("join request granted");
                    showJoinSuccess();

                    SearchPage nextPage = new SearchPage(message.Items[1].ToString());
                    await Navigation.PushAsync(nextPage, true);
                    

                    break;

                case "joinSearchRequestRejected":
                    Console.WriteLine("join request rejected");
                    showJoinRejected("Join request rejected! \n Search Code: " + message.Body + " is invalid!");

                    break;

                default:
                    Console.WriteLine("[MSG] joinSearchParametersPage recieved unknown message");
                    break;
            }
        }
    }
}
