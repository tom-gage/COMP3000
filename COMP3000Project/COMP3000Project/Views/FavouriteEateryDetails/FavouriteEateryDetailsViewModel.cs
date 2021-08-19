﻿using COMP3000Project.Interfaces;
using COMP3000Project.TestObjects;
using COMP3000Project.ViewModel;
using COMP3000Project.Views.Settings;
using COMP3000Project.WS;
using COMP3000Project.Views.Search;
using COMP3000Project.Views.SearchParameters;
using COMP3000Project.UserDetailsSingleton;
using COMP3000Project.Views.FavouriteEateries;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Text.Json;


namespace COMP3000Project.Views.FavouriteEateryDetails
{
    public class FavouriteEateryDetailsViewModel : ViewModelBase, Subscriber
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

        private string eateryAddress;
        public string EateryAddress
        {
            get { return eateryAddress; }
            set
            {
                if (eateryAddress != value)
                {
                    SetProperty(ref eateryAddress, value);//informs view of change
                }
            }
        }

        private string eateryPhoneNumber;
        public string EateryPhoneNumber
        {
            get { return eateryPhoneNumber; }
            set
            {
                if (eateryPhoneNumber != value)
                {
                    SetProperty(ref eateryPhoneNumber, value);//informs view of change
                }
            }
        }

        private string notes;
        public string Note
        {
            get { return notes; }
            set
            {
                if (notes != value)
                {
                    SetProperty(ref notes, value);//informs view of change
                }
            }
        }

        ObservableCollection<ImageHolder> images;
        public ObservableCollection<ImageHolder> Images { get => images; set => SetProperty(ref images, value); }

        ObservableCollection<Review> reviews;
        public ObservableCollection<Review> Reviews { get => reviews; set => SetProperty(ref reviews, value); }

        //COMMANDS
        public ICommand SaveNote { get; }
        public ICommand DeleteFavourite { get; }

        //constructor
        public FavouriteEateryDetailsViewModel(EateryOption eateryOption)
        {
            //set commands
            SaveNote = new Command(async () => ExecuteSaveNote());
            DeleteFavourite = new Command(async () => ExecuteDeleteFavourite());

            //init properties
            Images = new ObservableCollection<ImageHolder>();
            Reviews = eateryOption.Reviews;
            EateryTitle = eateryOption.Title;
            EateryRating = eateryOption.FormattedRating;
            EateryAddress = eateryOption.Address;
            EateryPhoneNumber = eateryOption.PhoneNumber;
            Note = eateryOption.Notes;

            //set text size
            VeryLarge = UserDetails.GetVeryLargeTextSetting();
            Large = UserDetails.GetlargeTextSetting();
            Medium = UserDetails.GetMediumTextSetting();
            Small = UserDetails.GetSmallTextSetting();

            //populate images array
            populateImagesArray(eateryOption);

        }

        //FUNCTIONS
        //save note
        public async void ExecuteSaveNote()
        {
            WebsocketHandler.RequestAddNoteToFavouriteEatery(UserDetails.Username, UserDetails.Password, EateryTitle, Note);
        }

        //delete favourite
        public async void ExecuteDeleteFavourite()
        {
            WebsocketHandler.RequestDeleteFavouriteEatery(UserDetails.Username, UserDetails.Password, EateryTitle);
        }

        //populate images array
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

        //catches incoming messages from the publisher
        async public void Update(Message message)
        {
            switch (message.type)
            {
                case "":
                    Console.WriteLine("...");

                    break;

                case "favouriteEateryDeleted":
                    //eatery deleted, return to favourites page
                    await Navigation.PopAsync();

                    break;

                case "noteUpdated":
                    Console.WriteLine("NOTE UPDATED!");
                    break;

                default:
                    Console.WriteLine("[MSG] main menu recieved unknown message");
                    break;
            }
        }
    }
}
