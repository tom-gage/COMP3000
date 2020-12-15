using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace COMP3000Project.TestObjects
{
    class EateryOption : INotifyPropertyChanged
    {
        //VARS
        public event PropertyChangedEventHandler PropertyChanged;

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
        public EateryOption(string title, string description, float rating, string photoReference)
        {
            this.Title = title;
            this.Description = description;
            this.Rating = rating;
            this.PhotoReference = photoReference;
        }


        //FUNCTIONS
        void HandlePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);

            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}
