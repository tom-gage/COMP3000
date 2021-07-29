using Newtonsoft.Json;
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
        private static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "flashCardData.json");



        //FUNCTIONS
        //get local data
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

                string[] readText = File.ReadAllLines(path);
                foreach (string s in readText)
                {
                    //string.Concat(content, s);
                    content = content + s;
                }

                userDetailsMap = JsonConvert.DeserializeObject<MapToUserDetails>(content);//create cardCollectionStore

                if (userDetailsMap == null)//if file is empty, return an empty collection store
                {
                    UserDetails.IsFirstStartUp = true;
                } else
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
            }
        }

        //sets user details
        public static void SetUserDetails(MapToUserDetails userDetailsMap)
        {
            UserDetails.Username = userDetailsMap.Username;
            UserDetails.Password = userDetailsMap.Password;
            UserDetails.SearchID = userDetailsMap.SearchID;
            UserDetails.IsFirstStartUp = userDetailsMap.IsFirstStartUp;
            UserDetails.PreferVeryLargeText = userDetailsMap.PreferVeryLargeText;
            UserDetails.PreferLargeText = userDetailsMap.PreferLargeText;
            UserDetails.PreferMediumText = userDetailsMap.PreferMediumText;
        }

        //gets a user details map object
        public static MapToUserDetails GetUserDetailsMap()
        {
            MapToUserDetails map = new MapToUserDetails();

            map.Username = UserDetails.Username;
            map.Password = UserDetails.Password;
            map.SearchID = UserDetails.SearchID;
            map.IsFirstStartUp = UserDetails.IsFirstStartUp;

            return map;
        }


        //save user details locally
        public static async Task SaveUserDetails()
        {
            JsonSerializer serializer = new JsonSerializer();//initialise serialiser

            using (StreamWriter sw = new StreamWriter(path))//initialise writer, targeting to local storage file

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, GetUserDetailsMap());//serialise data to file 
            }
        }

    }
}
