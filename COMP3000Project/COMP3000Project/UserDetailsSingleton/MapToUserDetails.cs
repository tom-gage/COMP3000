using System;
using System.Collections.Generic;
using System.Text;

namespace COMP3000Project.UserDetailsSingleton
{
    public class MapToUserDetails
    {
        //VARIABLES
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                }
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                }
            }
        }

        private string _searchID;

        public string SearchID
        {
            get { return _searchID; }
            set
            {
                if (_searchID != value)
                {
                    _searchID = value;
                }
            }
        }

        private bool isFirstStartUp;
        public bool IsFirstStartUp
        {
            get { return isFirstStartUp; }
            set
            {
                if (isFirstStartUp != value)
                {
                    isFirstStartUp = value;
                }
            }
        }

        private bool preferVeryLargeText;
        public bool PreferVeryLargeText
        {
            get { return preferVeryLargeText; }
            set
            {
                if (preferVeryLargeText != value)
                {
                    preferVeryLargeText = value;
                }
            }
        }

        private bool preferLargeText;
        public bool PreferLargeText
        {
            get { return preferLargeText; }
            set
            {
                if (preferLargeText != value)
                {
                    preferLargeText = value;
                }
            }
        }

        private bool preferMediumText;
        public bool PreferMediumText
        {
            get { return preferMediumText; }
            set
            {
                if (preferMediumText != value)
                {
                    preferMediumText = value;
                }
            }
        }
    }
}
