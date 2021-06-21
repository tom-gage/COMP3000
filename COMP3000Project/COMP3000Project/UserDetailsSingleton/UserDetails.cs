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
    }
}
