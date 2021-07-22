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
using System.Text.Json;
using COMP3000Project.TestObjects;
using COMP3000Project.Views.FavouriteEateryDetails;
using COMP3000Project.UserDetailsSingleton;
using System.Collections.ObjectModel;
using COMP3000Project.Views.SearchParameters;

namespace COMP3000Project.Views.FavouriteEateries
{
    class FavouriteEateriesViewModel : ViewModelBase, Subscriber
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

        private EateryOption _selectedEateryOption;
        public EateryOption SelectedEateryOption
        {
            get { return _selectedEateryOption; }
            set
            {
                if (_selectedEateryOption != value)
                {
                    SetProperty(ref _selectedEateryOption, value);//informs view of change
                }
            }
        }


        ObservableCollection<EateryOption> _favourites;
        public ObservableCollection<EateryOption> Favourites { get => _favourites; set => SetProperty(ref _favourites, value); }

        //COMMANDS
        public ICommand GoToFavouriteEateryDetailsPage { get; }

        //CONSTRUCTOR
        public FavouriteEateriesViewModel()
        {

            //set commands
            GoToFavouriteEateryDetailsPage = new Command(async () => await ExecuteGoToFavouriteEateryDetailsPage());



            WebsocketHandler.registerSubscriber(this);
        }

        //FUNCTIONS

        async Task<object> ExecuteGoToFavouriteEateryDetailsPage()
        {

            FavouriteEateryDetailsPage nextPage = new FavouriteEateryDetailsPage(SelectedEateryOption);

            await Navigation.PushAsync(nextPage, true);
            return null;
        }

        async void populateFavouritesArray(string optionsJSON)
        {
            Favourites = JsonSerializer.Deserialize<ObservableCollection<EateryOption>>(optionsJSON);
        }


        public void Update(Message message)
        {
            switch (message.type)
            {
                case "":

                    break;

                case "gotFavourites":
                    populateFavouritesArray(message.Items[0].ToString());
                    break;

                default:
                    Console.WriteLine("[MSG] favs page recieved unknown message");
                    break;
            }
        }
    }
}

