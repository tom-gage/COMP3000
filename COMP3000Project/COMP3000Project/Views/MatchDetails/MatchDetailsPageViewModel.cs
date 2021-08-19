﻿using COMP3000Project.Interfaces;
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
using System.Text.Json;
using COMP3000Project.UserDetailsSingleton;
using Xamarin.Forms.Maps;

namespace COMP3000Project.Views.MatchDetails
{
    class MatchDetailsPageViewModel : ViewModelBase, Subscriber
    {
        //VARIABLES
        Geocoder geoCoder;

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

        private double latitude;
        public double Latitude
        {
            get { return latitude; }
            set
            {
                if (latitude != value)
                {
                    SetProperty(ref latitude, value);//informs view of change
                }
            }
        }

        private double longitude;
        public double Longitude
        {
            get { return longitude; }
            set
            {
                if (longitude != value)
                {
                    SetProperty(ref longitude, value);//informs view of change
                }
            }
        }

        private Map map;
        public Map Map
        {
            get { return map; }
            set
            {
                if (map != value)
                {
                    SetProperty(ref map, value);//informs view of change
                }
            }
        }

        ObservableCollection<ImageHolder> images;
        public ObservableCollection<ImageHolder> Images { get => images; set => SetProperty(ref images, value); }

        //COMMANDS


        //CONSTRUCTOR
        public MatchDetailsPageViewModel(EateryOption eateryOption)
        {
            //set properties
            Images = new ObservableCollection<ImageHolder>();
            Title = eateryOption.Title;
            Rating = eateryOption.Rating.ToString();
            EateryAddress = eateryOption.Address;
            EateryPhoneNumber = eateryOption.PhoneNumber;

            geoCoder = new Geocoder();

            Latitude = 0; //36.9628066;
            Longitude = 0; // -122.0194722;

            //populate images array
            populateImagesArray(eateryOption);


            //set text size
            VeryLarge = UserDetails.GetVeryLargeTextSetting();
            Large = UserDetails.GetlargeTextSetting();
            Medium = UserDetails.GetMediumTextSetting();
            Small = UserDetails.GetSmallTextSetting();

        }

        //FUNCTIONS
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

        public async void GetCoordinates()
        {

            var approximateLocation = await geoCoder.GetPositionsForAddressAsync(EateryAddress);

            foreach (var p in approximateLocation)
            {
                Latitude = p.Latitude;
                Longitude = p.Longitude;

                

            }

            Map = new Map(MapSpan.FromCenterAndRadius(new Position(Latitude, Longitude), Distance.FromKilometers(0.3)));
            
            Pin pin = new Pin
            {
                Label = Title,
                Address = EateryAddress,
                Type = PinType.Place,
                Position = new Position(Latitude, Longitude)
            };

            Map.Pins.Add(pin);

        }

        //catches incoming messages from the publisher
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
