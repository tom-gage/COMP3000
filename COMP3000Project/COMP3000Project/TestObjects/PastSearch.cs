using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace COMP3000Project.TestObjects
{
    public class PastSearch
    {
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

        string _location;
        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                if (_location == value)
                {
                    return;
                }
                _location = value;
                HandlePropertyChanged();
            }
        }

        string _formattedLocation;
        public string FormattedLocation
        {
            get
            {
                return "Location: "+ Location;
            }
            set
            {
                if (_formattedLocation == value)
                {
                    return;
                }
                _formattedLocation = value;
                HandlePropertyChanged();
            }
        }

        string _time;
        public string Time
        {
            get
            {
                return _time;
            }
            set
            {
                if (_time == value)
                {
                    return;
                }
                _time = value;
                HandlePropertyChanged();
            }
        }

        TimeSpan _timeSpan;
        public TimeSpan timeSpan
        {
            get
            {
                return TimeSpan.Parse(Time[0].ToString() + Time[1].ToString() + ":" + Time[2].ToString() + Time[3].ToString());
                //return TimeSpan.Parse(Time[0].ToString() + Time[1].ToString());
            }
            set
            {
                if (_time == value.ToString("hhmm"))
                {
                    return;
                }
                _time = value.ToString("hhmm");
                HandlePropertyChanged();
            }
        }

        string _eateryType;
        public string EateryType
        {
            get
            {
                return _eateryType;
            }
            set
            {
                if (_eateryType == value)
                {
                    return;
                }
                _eateryType = value;
                HandlePropertyChanged();
            }
        }

        string _formattedEateryType;
        public string FormattedEateryType
        {
            get
            {
                return "Eatery type: " + EateryType;
            }
            set
            {
                if (_formattedEateryType == value)
                {
                    return;
                }
                _formattedEateryType = value;
                HandlePropertyChanged();
            }
        }

        string dayOfSearch;
        public string DayOfSearch
        {
            get
            {
                return dayOfSearch;
            }
            set
            {
                if (dayOfSearch == value)
                {
                    return;
                }
                dayOfSearch = value;
                HandlePropertyChanged();
            }
        }

        string monthOfSearch;
        public string MonthOfSearch
        {
            get
            {
                return monthOfSearch;
            }
            set
            {
                if (monthOfSearch == value)
                {
                    return;
                }
                monthOfSearch = value;
                HandlePropertyChanged();
            }
        }

        string yearOfSearch;
        public string YearOfSearch
        {
            get
            {
                return yearOfSearch;
            }
            set
            {
                if (yearOfSearch == value)
                {
                    return;
                }
                yearOfSearch = value;
                HandlePropertyChanged();
            }
        }

        string formattedDateOfSearch;
        public string FormattedDateOfSearch
        {
            get { return DayOfSearch + "/" + MonthOfSearch + "/" + YearOfSearch; }
            set
            {
                if (formattedDateOfSearch == value)
                {
                    return;
                }
                formattedDateOfSearch = value;
                HandlePropertyChanged();
            }
        }


        void HandlePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);

            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}
