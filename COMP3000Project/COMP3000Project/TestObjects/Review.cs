using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace COMP3000Project.TestObjects
{
    public class Review : INotifyPropertyChanged
    {
        //VARS
        public event PropertyChangedEventHandler PropertyChanged;

        //"author_name" : "Robert Ardill",
        //    "author_url" : "https://www.google.com/maps/contrib/106422854611155436041/reviews",
        //    "language" : "en",
        //    "profile_photo_url" : "https://lh3.googleusercontent.com/-T47KxWuAoJU/AAAAAAAAAAI/AAAAAAAAAZo/BDmyI12BZAs/s128-c0x00000000-cc-rp-mo-ba1/photo.jpg",
        //    "rating" : 5,
        //    "relative_time_description" : "a month ago",
        //    "text" : "Awesome offices. Great facilities, location and views. Staff are great hosts",
        //    "time" : 1491144016

        string authorName;

        public string AuthorName
        {
            get
            {
                return authorName;
            }
            set
            {
                if (authorName == value)
                {
                    return;
                }
                authorName = value;
                HandlePropertyChanged();
            }
        }

        string rating;

        public string Rating
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
                return rating + "/5";
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

        string relativeTimeDescription;

        public string RelativeTimeDescription
        {
            get
            {
                return relativeTimeDescription;
            }
            set
            {
                if (relativeTimeDescription == value)
                {
                    return;
                }
                relativeTimeDescription = value;
                HandlePropertyChanged();
            }
        }

        string text;

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (text == value)
                {
                    return;
                }
                text = value;
                HandlePropertyChanged();
            }
        }

        int timeSinceReview;

        public int TimeSinceReview
        {
            get
            {
                return timeSinceReview;
            }
            set
            {
                if (timeSinceReview == value)
                {
                    return;
                }
                timeSinceReview = value;
                HandlePropertyChanged();
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
