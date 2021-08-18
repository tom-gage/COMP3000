﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using COMP3000Project.UserDetailsSingleton;



namespace COMP3000Project.LDH
{
    public static class LocalDataHandler
    {
        //PROPERTIES
        private static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "userDetails.json");



        //FUNCTIONS
        //gets user details data from local storage
        public static void SetUserDetailsFromLocalStorage()
        {
            MapToUserDetails userDetailsMap;
            string content = "";

            try
            {
                if (!File.Exists(path))//if local storage file does not exist
                {
                    File.Create(path);//create file
                }

                //read the contents of the file
                string[] readText = File.ReadAllLines(path);
                foreach (string s in readText)
                {
                    content = content + s;
                }

                userDetailsMap = JsonConvert.DeserializeObject<MapToUserDetails>(content);//use contents to make a userDetailsMap

                if (userDetailsMap == null)//if file is empty, first start up is true
                {
                    UserDetails.IsFirstStartUp = true;
                    UserDetails.SearchPageTutorialShown = false;
                } else //else, set user details from map
                {
                    SetUserDetails(userDetailsMap);
                }

            }
            catch
            {
                UserDetails.Username = "";
                UserDetails.Password = "";
                UserDetails.SearchID = "";
                UserDetails.IsFirstStartUp = true;
                UserDetails.SearchPageTutorialShown = false;
            }
        }

        //sets user details
        static void SetUserDetails(MapToUserDetails userDetailsMap)
        {
            UserDetails.Username = userDetailsMap.Username;
            UserDetails.Password = userDetailsMap.Password;
            UserDetails.SearchID = userDetailsMap.SearchID;
            UserDetails.IsFirstStartUp = userDetailsMap.IsFirstStartUp;
            UserDetails.PreferVeryLargeText = userDetailsMap.PreferVeryLargeText;
            UserDetails.PreferLargeText = userDetailsMap.PreferLargeText;
            UserDetails.PreferMediumText = userDetailsMap.PreferMediumText;
            UserDetails.SearchPageTutorialShown = userDetailsMap.SearchPageTutorialShown;
        }

        //gets a user details map object
        static MapToUserDetails GetUserDetailsMap()
        {
            MapToUserDetails map = new MapToUserDetails();

            map.Username = UserDetails.Username;
            map.Password = UserDetails.Password;
            map.SearchID = UserDetails.SearchID;
            map.IsFirstStartUp = UserDetails.IsFirstStartUp;
            map.PreferVeryLargeText = UserDetails.PreferVeryLargeText;
            map.PreferLargeText = UserDetails.PreferLargeText;
            map.PreferMediumText = UserDetails.PreferMediumText;
            map.SearchPageTutorialShown = UserDetails.SearchPageTutorialShown;

            return map;
        }


        //save the local store of user details
        public static async Task SaveUserDetails()
        {
            JsonSerializer serializer = new JsonSerializer();//initialise serialiser

            using (StreamWriter sw = new StreamWriter(path))//initialise writer, targeting to local storage file

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, GetUserDetailsMap());//serialise data to file 
            }
        }

        //wipe the local store of user details
        public static async Task WipeUserDetails()
        {
            JsonSerializer serializer = new JsonSerializer();//initialise serialiser

            using (StreamWriter sw = new StreamWriter(path))//initialise writer, targeting to local storage file

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, null);//serialise data to file 
            }
        }
    }
}
