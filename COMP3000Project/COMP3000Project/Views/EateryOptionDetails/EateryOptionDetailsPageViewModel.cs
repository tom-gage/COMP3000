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
        private string eateryTitle;
        public string EateryTitle
        {
            get { return eateryTitle; }
            set
            {
                if (eateryTitle != value)
                {
                    SetProperty(ref eateryTitle, value);//informs view of change
                }
            }
        }

        private string eateryRating;
        public string EateryRating
        {
            get { return eateryRating; }
            set
            {
                if (eateryRating != value)
                {
                    SetProperty(ref eateryRating, value);//informs view of change
                }
            }
        }

        ObservableCollection<ImageHolder> images;
        public ObservableCollection<ImageHolder> Images { get => images; set => SetProperty(ref images, value); }

        //COMMANDS
        public ICommand GoToStartSearch { get; }


        //CONSTRUCTOR
        public EateryOptionDetailsPageViewModel(EateryOption eateryOption)
        {

            //set commands
            //GoToStartSearch = new Command(async () => await GoToExecuteStartSearch());

            Images = new ObservableCollection<ImageHolder>();

            EateryTitle = eateryOption.Title;
            EateryRating = eateryOption.Rating.ToString() + "/5";
            EateryRating = "ass";

            populateImagesArray(eateryOption);

            WebsocketHandler.registerSubscriber(this);
        }

        //FUNCTIONS
        async void populateImagesArray(EateryOption eateryOption)
        {
            if (eateryOption.EateryImage0 != null)
            {
                Images.Add(new ImageHolder(eateryOption.EateryImage0));
            }

            if (eateryOption.EateryImage1 != null)
            {
                Images.Add(new ImageHolder(eateryOption.EateryImage1));
            }

            if (eateryOption.EateryImage2 != null)
            {
                Images.Add(new ImageHolder(eateryOption.EateryImage2));
            }

            if (eateryOption.EateryImage3 != null)
            {
                Images.Add(new ImageHolder(eateryOption.EateryImage3));
            }

            if (eateryOption.EateryImage4 != null)
            {
                Images.Add(new ImageHolder(eateryOption.EateryImage4));
            }
        }

        public void Update(Message message)
        {
            switch (message.type)
            {
                case "":
                    Console.WriteLine("...");

                    break;


                default:
                    Console.WriteLine("[MSG] eatery option details page recieved unknown message");
                    break;
            }
        }
    }
}
