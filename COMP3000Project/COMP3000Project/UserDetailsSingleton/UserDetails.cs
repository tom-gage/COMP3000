using System;
using System.Collections.Generic;
using System.Text;

namespace COMP3000Project.UserDetailsSingleton
{
    public static class UserDetails
    {
        //VARIABLES
        private static string _username;
        public static string Username
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

        private static string _password;

        public static string Password
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

        private static string _searchID;

        public static string SearchID
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

        private static bool isFirstStartUp;
        public static bool IsFirstStartUp
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

        private static bool preferVeryLargeText;
        public static bool PreferVeryLargeText
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

        private static bool preferLargeText;
        public static bool PreferLargeText
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

        private static bool preferMediumText;
        public static bool PreferMediumText
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



        //functions
        public static void setDetails(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public static void clearDetails()
        {
            Username = null;
            Password = null;
        }

        public static float GetVeryLargeTextSetting()
        {
            if (PreferVeryLargeText)
            {
                return 40;
            }
            if (PreferLargeText)
            {
                return 35;
            }
            if (PreferMediumText)
            {
                return 30;
            }

            return 30;
        }

        public static float GetlargeTextSetting()
        {
            if (PreferVeryLargeText)
            {
                return 32;
            }
            if (PreferLargeText)
            {
                return 27;
            }
            if (PreferMediumText)
            {
                return 22;
            }

            return 22;
        }

        public static float GetMediumTextSetting()
        {
            if (PreferVeryLargeText)
            {
                return 27;
            }
            if (PreferLargeText)
            {
                return 22;
            }
            if (PreferMediumText)
            {
                return 17;
            }

            return 17;
        }

        public static double GetSmallTextSetting()
        {

            if (PreferVeryLargeText)
            {
                return 22;
            }
            if (PreferLargeText)
            {
                return 17;
            }
            if (PreferMediumText)
            {
                return 14;
            }

            return 14;
        }
    }
}
