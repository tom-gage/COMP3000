using COMP3000Project.Interfaces;
using COMP3000Project.TestObjects;
using COMP3000Project.ViewModel;
using COMP3000Project.Views.Settings;
using COMP3000Project.WS;
using COMP3000Project.Views.Search;
using COMP3000Project.Views.SearchParameters;
using COMP3000Project.UserDetailsSingleton;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace COMP3000Project.Views.EateryOptionDetails
{
    class EateryOptionDetailsPageViewModel : ViewModelBase, Subscriber
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


        //CONSTRUCTOR
        public EateryOptionDetailsPageViewModel(EateryOption eateryOption)
        {

            //set commands
            //GoToStartSearch = new Command(async () => await GoToExecuteStartSearch());

            WebsocketHandler.registerSubscriber(this);
        }

        //FUNCTIONS
        async void populatePastSearchesArray(string optionsJSON)
        {
            //PastSearches = JsonSerializer.Deserialize<ObservableCollection<PastSearch>>(optionsJSON);
        }

        public void Update(Message message)
        {
            switch (message.type)
            {
                case "":
                    Console.WriteLine("...");

                    break;

                case "gotPastSearches":
                    Console.WriteLine("[MSG] GOT PAST SEARCHES!");
                    Console.WriteLine(message.Items[0]);
                    populatePastSearchesArray(message.Items[0].ToString());

                    //now populate past searches array...


                    break;

                default:
                    Console.WriteLine("[MSG] main menu recieved unknown message");
                    break;
            }
        }
    }
}
