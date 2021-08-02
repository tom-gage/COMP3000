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

            //set text size
            VeryLarge = UserDetails.GetVeryLargeTextSetting();
            Large = UserDetails.GetlargeTextSetting();
            Medium = UserDetails.GetMediumTextSetting();
            Small = UserDetails.GetSmallTextSetting();

            //register this class as a subscriber to the websocket handler, allows for the recieving of inter class messages
            WebsocketHandler.registerSubscriber(this);
        }



        //FUNCTIONS
        //attempt to join a search
        async Task<object> ExecuteJoinSearch()
        {
            if (ValidateSearchCode(SearchCode))//if code exists
            {
                //show join in progress feedback
                showFeedBacktext("Joining a search...", "Orange");
                //begin request
                WebsocketHandler.RequestJoinExistingSearch(UserDetails.Username, UserDetails.Password, SearchCode);

            } else
            {
                //show empty code request
                showFeedBacktext("Please enter a join code!", "Red");
                
            }
            return null;
        }

        //hide feedback text
        public void hideFeedbackText()
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

        //if search code entered is null or empty, return false
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

        //catches incoming messages from the publisher
        async public void Update(Message message)
        {
            switch (message.type)
            {
                case "":
                    Console.WriteLine("...");

                    break;

                case "joinSearchRequestGranted"://join search is successful
                    Console.WriteLine("join request granted");
                    showFeedBacktext("Join search successful!", "Green");

                    SearchPage nextPage = new SearchPage(message.Items[1].ToString());//navigate to search page
                    await Navigation.PushAsync(nextPage, true);
                    

                    break;

                case "joinSearchRequestRejected"://join search unsuccessful
                    Console.WriteLine("join request rejected");
                    showFeedBacktext("Join request rejected! \n Search Code: " + message.Body + " is invalid!", "Red");//show negative feedback

                    break;

                default:
                    Console.WriteLine("[MSG] joinSearchParametersPage recieved unknown message");
                    break;
            }
        }
    }
}
