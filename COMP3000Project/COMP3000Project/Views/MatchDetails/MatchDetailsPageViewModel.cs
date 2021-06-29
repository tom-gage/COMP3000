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

namespace COMP3000Project.Views.MatchDetails
{
    class MatchDetailsPageViewModel : ViewModelBase, Subscriber
    {
        //VARIABLES
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    SetProperty(ref _title, value);//informs view of change
                }
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    SetProperty(ref _description, value);//informs view of change
                }
            }
        }

        private string _rating;
        public string Rating
        {
            get { return _rating; }
            set
            {
                if (_rating != value)
                {
                    SetProperty(ref _rating, value);//informs view of change
                }
            }
        }

        ImageSource _eateryImage;

        public ImageSource EateryImage
        {
            get
            {
                return _eateryImage;
            }
            set
            {
                if (_eateryImage == value)
                {
                    return;
                }
                _eateryImage = value;
                HandlePropertyChanged();
            }
        }

        //COMMANDS
        public ICommand GoToStartSearch { get; }

        //CONSTRUCTOR
        public MatchDetailsPageViewModel(EateryOption eateryOption)
        {
            EateryImage = eateryOption.EateryImage;
            Title = eateryOption.Title;
            Description = eateryOption.Description;
            Rating = eateryOption.Rating.ToString();

            //set commands
            GoToStartSearch = new Command(async () => await GoToExecuteStartSearch());

            WebsocketHandler.registerSubscriber(this);
        }

        //FUNCTIONS
        async Task<object> GoToExecuteStartSearch()
        {
            bool startingNewSearch = true;
            SearchPage nextPage = new SearchPage(startingNewSearch, "");
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
                    Console.WriteLine("[MSG] match details page recieved unknown message");
                    break;
            }
        }
    }
}
