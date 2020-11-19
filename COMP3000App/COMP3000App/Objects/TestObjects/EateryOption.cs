using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace COMP3000App.Objects.TestObjects
{
    class EateryOption : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                //OnPropertyChanged();
            }
        }
        //public string title { get; set; }
        public string description { get; set; }
        public float rating { get; set; }
        public EateryOption(string title, string description, float rating)
        {
            this.title = title;
            this.description = description;
            this.rating = rating;
        }

        protected void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, null);
        }


    }
}
