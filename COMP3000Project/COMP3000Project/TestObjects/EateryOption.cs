using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace COMP3000Project.TestObjects
{
    public class EateryOption : INotifyPropertyChanged
    {
        //EateryOption, represents a real-world location that a user can encounter, vote for, and save to their favourites

        //VARS
        public event PropertyChangedEventHandler PropertyChanged;

        string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                if (_username == value)
                {
                    return;
                }
                _username = value;
                HandlePropertyChanged();
            }
        }

        string notes;
        public string Notes
        {
            get
            {
                return notes;
            }
            set
            {
                if (notes == value)
                {
                    return;
                }
                notes = value;
                HandlePropertyChanged();
            }
        }

        string _id;
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id == value)
                {
                    return;
                }
                _id = value;
                HandlePropertyChanged();
            }
        }

        string _title;
        public string Title {
            get
            {
                return _title;
            }
            set
            {
                if(_title == value)
                {
                    return;
                }
                _title = value;
                HandlePropertyChanged();
            }
        }

        string description;

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description == value)
                {
                    return;
                }
                description = value;
                HandlePropertyChanged();
            }
        }
        float rating;

        public float Rating
        {
            get
            {
                return rating;
            }
            set
            {
                if (rating == value)
                {
                    return;
                }
                rating = value;
                HandlePropertyChanged();
            }
        }

        string formattedRating;

        public string FormattedRating
        {
            get
            {
                return rating.ToString() + "/5";
            }
            set
            {
                if (formattedRating == value)
                {
                    return;
                }
                formattedRating = value;
                HandlePropertyChanged();
            }
        }

        string photoReference0;
        public string PhotoReference0
        {
            get
            {
                return photoReference0;
            }
            set
            {
                if (photoReference0 == value)
                {
                    return;
                }
                photoReference0 = value;
                HandlePropertyChanged();
            }
        }

        string photoReference1;
        public string PhotoReference1
        {
            get
            {
                return photoReference1;
            }
            set
            {
                if (photoReference1 == value)
                {
                    return;
                }
                photoReference1 = value;
                HandlePropertyChanged();
            }
        }

        string photoReference2;
        public string PhotoReference2
        {
            get
            {
                return photoReference2;
            }
            set
            {
                if (photoReference2 == value)
                {
                    return;
                }
                photoReference2 = value;
                HandlePropertyChanged();
            }
        }

        string photoReference3;
        public string PhotoReference3
        {
            get
            {
                return photoReference3;
            }
            set
            {
                if (photoReference3 == value)
                {
                    return;
                }
                photoReference3 = value;
                HandlePropertyChanged();
            }
        }

        string photoReference4;
        public string PhotoReference4
        {
            get
            {
                return photoReference4;
            }
            set
            {
                if (photoReference4 == value)
                {
                    return;
                }
                photoReference4 = value;
                HandlePropertyChanged();
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
                if(_eateryImage == value)
                {
                    return;
                }
                _eateryImage = value;
                HandlePropertyChanged();
            }
        }

        ImageSource _eateryImage0;

        public ImageSource EateryImage0
        {
            get
            {
                return _eateryImage0;
            }
            set
            {
                if (_eateryImage0 == value)
                {
                    return;
                }
                _eateryImage0 = value;
                HandlePropertyChanged();
            }
        }

        ImageSource _eateryImage1;

        public ImageSource EateryImage1
        {
            get
            {
                return _eateryImage1;
            }
            set
            {
                if (_eateryImage1 == value)
                {
                    return;
                }
                _eateryImage1 = value;
                HandlePropertyChanged();
            }
        }

        ImageSource _eateryImage2;

        public ImageSource EateryImage2
        {
            get
            {
                return _eateryImage2;
            }
            set
            {
                if (_eateryImage2 == value)
                {
                    return;
                }
                _eateryImage2 = value;
                HandlePropertyChanged();
            }
        }

        ImageSource _eateryImage3;

        public ImageSource EateryImage3
        {
            get
            {
                return _eateryImage3;
            }
            set
            {
                if (_eateryImage3 == value)
                {
                    return;
                }
                _eateryImage3 = value;
                HandlePropertyChanged();
            }
        }

        ImageSource _eateryImage4;

        public ImageSource EateryImage4
        {
            get
            {
                return _eateryImage4;
            }
            set
            {
                if (_eateryImage4 == value)
                {
                    return;
                }
                _eateryImage4 = value;
                HandlePropertyChanged();
            }
        }

        ObservableCollection<Review> reviews;
        public ObservableCollection<Review> Reviews
        {
            get
            {
                return reviews;
            }
            set
            {
                if (reviews == value)
                {
                    return;
                }
                reviews = value;
                HandlePropertyChanged();
            }
        }

        //Review[] reviews;
        //public Review[] Reviews
        //{
        //    get
        //    {
        //        return reviews;
        //    }
        //    set
        //    {
        //        if (reviews == value)
        //        {
        //            return;
        //        }
        //        reviews = value;
        //        HandlePropertyChanged();
        //    }
        //}



        string[] _votes;
        public string[] Votes
        {
            get
            {
                return _votes;
            }
            set
            {
                if (_votes == value)
                {
                    return;
                }
                _votes = value;
                HandlePropertyChanged();
            }
        }

        string _openingTime;
        public string OpeningTime
        {
            get
            {
                return _openingTime;
            }
            set
            {
                if (_openingTime == value)
                {
                    return;
                }
                _openingTime = value;
                HandlePropertyChanged();
            }
        }

        string _closingTime;
        public string ClosingTime
        {
            get
            {
                return _closingTime;
            }
            set
            {
                if (_closingTime == value)
                {
                    return;
                }
                _closingTime = value;
                HandlePropertyChanged();
            }
        }

        string _timeToClosingTime;
        public string TimeToClosingTime
        {
            get
            {
                if(_timeToClosingTime == "0")
                {
                    return "Open Now!";
                }

                return _timeToClosingTime + " hours to closing time (from stated arrival time).";
            }
            set
            {
                if (_timeToClosingTime == value)
                {
                    return;
                }
                _timeToClosingTime = value;
                HandlePropertyChanged();
            }
        }

        string _timeToCloseColourIndicator;
        public string TimeToCloseColourIndicator
        {
            get
            {
                if (TimeToClosingTime == "0 hours to closing time (from stated arrival time).")
                {
                    return "Green";
                }
                else if (TimeToClosingTime == "1 hours to closing time (from stated arrival time).")
                {
                    return "Red";
                }
                else if (TimeToClosingTime == "2 hours to closing time (from stated arrival time).")
                {
                    return "Orange";
                }


                return "Green";
            }
            set
            {
                if (_timeToCloseColourIndicator == value)
                {
                    return;
                }
                _timeToCloseColourIndicator = value;
                HandlePropertyChanged();
            }
        }

        string address;
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                if (address == value)
                {
                    return;
                }
                address = value;
                HandlePropertyChanged();
            }
        }


        string phoneNumber;
        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }
            set
            {
                if (phoneNumber == value)
                {
                    return;
                }
                phoneNumber = value;
                HandlePropertyChanged();
            }
        }

        public EateryOption(string ID, string title, string description, float rating, string photoReference0, string photoReference1, string photoReference2, string photoReference3, string photoReference4, string[] Votes)
        {
            this.ID = ID;
            this.Title = title;
            this.Description = description;
            this.Rating = rating;
            this.photoReference0 = photoReference0;
            this.photoReference1 = photoReference1;
            this.photoReference2 = photoReference2;
            this.photoReference3 = photoReference3;
            this.photoReference4 = photoReference4;
            this.Votes = Votes;

            ////sets the eatery image(s)
            var webImage = new Image { Aspect = Aspect.AspectFit };


            //believe me, i wanted to put this in an array but it crashed the websocket handler
            webImage.Source = ImageSource.FromUri(new Uri("https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=" + photoReference0 + "&key=AIzaSyBbIr0ggukOfFiCFLoQcpypMmhA5NAYCZw"));
            EateryImage = webImage.Source;

            EateryImage0 = null;
            EateryImage1 = null;
            EateryImage2 = null;
            EateryImage3 = null;
            EateryImage4 = null;

            if (photoReference0 != "")
            {
                webImage.Source = ImageSource.FromUri(new Uri("https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=" + photoReference0 + "&key=AIzaSyBbIr0ggukOfFiCFLoQcpypMmhA5NAYCZw"));
                EateryImage0 = webImage.Source;
            } 

            if (photoReference1 != "")
            {
                webImage.Source = ImageSource.FromUri(new Uri("https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=" + photoReference1 + "&key=AIzaSyBbIr0ggukOfFiCFLoQcpypMmhA5NAYCZw"));
                EateryImage1 = webImage.Source;
            }

            if (photoReference2 != "")
            {
                webImage.Source = ImageSource.FromUri(new Uri("https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=" + photoReference2 + "&key=AIzaSyBbIr0ggukOfFiCFLoQcpypMmhA5NAYCZw"));
                EateryImage2 = webImage.Source;
            }

            if (photoReference3 != "")
            {
                webImage.Source = ImageSource.FromUri(new Uri("https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=" + photoReference3 + "&key=AIzaSyBbIr0ggukOfFiCFLoQcpypMmhA5NAYCZw"));
                EateryImage3 = webImage.Source;
            }

            if (photoReference4 != "")
            {
                webImage.Source = ImageSource.FromUri(new Uri("https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=" + photoReference4 + "&key=AIzaSyBbIr0ggukOfFiCFLoQcpypMmhA5NAYCZw"));
                EateryImage4 = webImage.Source;
            }

        }


        //FUNCTIONS

        void HandlePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);

            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}
