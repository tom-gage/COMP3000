using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace COMP3000Project.TestObjects
{
    public class EateryOption : INotifyPropertyChanged
    {
        //VARS
        public event PropertyChangedEventHandler PropertyChanged;

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

        string photoReference;
        public string PhotoReference
        {
            get
            {
                return photoReference;
            }
            set
            {
                if (photoReference == value)
                {
                    return;
                }
                photoReference = value;
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

        public EateryOption(string ID, string title, string description, float rating, string photoReference, string[] Votes)
        {
            this.ID = ID;
            this.Title = title;
            this.Description = description;
            this.Rating = rating;
            this.PhotoReference = photoReference;
            this.Votes = Votes;

            //FOR TESTING PURPOSES
            var webImage = new Image { Aspect = Aspect.AspectFit };
            webImage.Source = ImageSource.FromUri(new Uri("https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=" + PhotoReference + "&key=AIzaSyBbIr0ggukOfFiCFLoQcpypMmhA5NAYCZw"));

            EateryImage = webImage.Source;
        }


        //FUNCTIONS
        void HandlePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);

            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}
