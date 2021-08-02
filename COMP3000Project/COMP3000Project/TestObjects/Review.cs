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
        //Review, holds the data for the reviews of eateries
        //VARS
        public event PropertyChangedEventHandler PropertyChanged;


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
