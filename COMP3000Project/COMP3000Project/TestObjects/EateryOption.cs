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

        public string description { get; set; }
        public float rating { get; set; }
        public EateryOption(string title, string description, float rating)
        {
            this.Title = title;
            this.description = description;
            this.rating = rating;
        }


        //FUNCTIONS
        void HandlePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);

            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}
