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


        void HandlePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);

            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}
