using COMP3000Project.TestObjects;
using COMP3000Project.Interfaces;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace COMP3000Project.WS
{


    public static class WebsocketHandler
    {
        
        static List<Subscriber> subscribers = new List<Subscriber>();

        static ClientWebSocket ws = new ClientWebSocket();


        //FUNCTIONS
        //initialises connection to the server
        public static async Task InitialiseConnectionAsync()
        {
            await ws.ConnectAsync(new Uri("ws://10.0.2.2:9000"), CancellationToken.None);

            await HandleMessages();

        }

        //asks the server to delete favourite eatery
        public static async void RequestDeleteFavouriteEatery(string username, string password, string eateryTitle)
        {
            string[] messageItems = { username, password, eateryTitle };

            //make request message object
            Message request = new Message("1", "deleteFavouriteEatery", "", messageItems);

            //send to server
            SendRequest(request);
        }


        //asks the server to add note to favourite
        public static async void RequestAddNoteToFavouriteEatery(string username, string password, string eateryTitle, string note)
        {
            string[] messageItems = { username, password, eateryTitle, note };

            //make request message object
            Message request = new Message("1", "updateNote", "", messageItems);

            //send to server
            SendRequest(request);
        }



        //asks the server to add eatery to favs
        public static async void RequestGetFavourites(string username, string password)
        {
            string[] messageItems = { username, password };

            //make request message object
            Message request = new Message("1", "getFavourites", "", messageItems);

            //send to server
            SendRequest(request);
        }

        //asks the server to add eatery to favs
        public static async void RequestAddToFavourites(string username, string password, EateryOption eateryOption)
        {
            TransmittableEateryOption x = new TransmittableEateryOption(username, eateryOption.ID, eateryOption.Title, eateryOption.Description, eateryOption.Rating, eateryOption.PhotoReference0, eateryOption.PhotoReference1, eateryOption.PhotoReference2, eateryOption.PhotoReference3, eateryOption.PhotoReference4, new List<Review>(eateryOption.Reviews).ToArray(), eateryOption.Votes, eateryOption.OpeningTime, eateryOption.ClosingTime, "0", eateryOption.Address, eateryOption.PhoneNumber);

            string y = JsonConvert.SerializeObject(x);
            string[] messageItems = { username, password, y};

            //make request message object
            Message request = new Message("1", "addToFavourites", "", messageItems);

            //send to server
            SendRequest(request);
        }


        //asks the server to get past searches
        public static async void RequestGetPastSearches(string username, string password)
        {
            string[] messageItems = { username, password };

            //make request message object
            Message request = new Message("1", "getPastSearches", "", messageItems);

            //send to server
            SendRequest(request);
        }

        //asks the server to register a new user
        public static async void RequestRegisterNewUser(string username, string password)
        {
            string[] messageItems = { username, password };

            //make request message object
            Message request = new Message("1", "registerNewUser", "", messageItems);

            //send to server
            SendRequest(request);
        }

        //asks the server to log in an existing user
        public static async void RequestLoginExistingUser(string username, string password)
        {
            string[] messageItems = { username, password };

            //make request message object
            Message request = new Message("1", "loginExistingUser", "", messageItems);

            //send to server
            SendRequest(request);
        }

        //asks the server to update the username of a user
        public static async void RequestChangeUsername(string currentUsername, string currentPassword, string newUsername)
        {
            string[] messageItems = { currentUsername, currentPassword, newUsername };

            //make request message object
            Message request = new Message("1", "updateUsername", "", messageItems);

            //send to server
            SendRequest(request);
        }

        //asks the server to update the password of a user
        public static async void RequestChangePassword(string currentUsername, string currentPassword, string newPassword)
        {
            string[] messageItems = { currentUsername, currentPassword, newPassword };

            //make request message object
            Message request = new Message("1", "updatePassword", "", messageItems);

            //send to server
            SendRequest(request);
        }

        //asks the server to delete a user
        public static async void RequestDeleteUser(string currentUsername, string currentPassword)
        {
            string[] messageItems = { currentUsername, currentPassword };

            //make request message object
            Message request = new Message("1", "deleteUser", "", messageItems);

            //send to server
            SendRequest(request);
        }

        //asks the server to start a new search
        public static async void RequestStartNewSearch(string currentUsername, string currentPassword, string locationName, string desiredTime, string[] eateryTypes)
        {
            object[] messageItems = { currentUsername, currentPassword, locationName, desiredTime, eateryTypes };

            //make request message object
            Message request = new Message("1", "startNewSearch", "", messageItems);

            //send to server
            SendRequest(request);
        }


        //public static async void RequestStartNewSearch(string currentUsername, string currentPassword, double lattitude, double longitude)
        //{
        //    object[] messageItems = { currentUsername, currentPassword, lattitude, longitude };

        //    //make request message object
        //    Message request = new Message("1", "startNewSearch", "", messageItems);

        //    //send to server
        //    SendRequest(request);
        //}

        //asks the server to join an existing search
        public static async void RequestJoinExistingSearch(string currentUsername, string currentPassword, string searchCode)
        {
            object[] messageItems = { currentUsername, currentPassword, searchCode };

            //make request message object
            Message request = new Message("1", "joinExistingSearch", "", messageItems);

            //send to server
            SendRequest(request);
        }

        //asks the server to accept a vote for an option
        public static async void RequestCastVote(string currentUsername, string currentPassword, string searchID, string eateryOptionID)
        {
            object[] messageItems = { currentUsername, currentPassword, searchID, eateryOptionID };

            //make request message object
            Message request = new Message("1", "castVote", "", messageItems);

            //send to server
            SendRequest(request);
        }


        public static async void SendRequest(Message message)
        {
            string jsonData = JsonConvert.SerializeObject(message);
            var encodedData = Encoding.UTF8.GetBytes(jsonData);
            var buffer = new ArraySegment<Byte>(encodedData, 0, encodedData.Length);
            await ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }


        //Publisher / Subscriber stuff, handles inter-class communication
        //register subscriber, registered subscribers can recieve message updates
        public static void registerSubscriber(Subscriber subscriber)
        {
            subscribers.Add(subscriber);
        }

        //remove subscriber
        public static void removeSubsciber(Subscriber subscriber)
        {
            subscribers.Remove(subscriber);
        }

        //update subscriber, calls the update function in the subscribed class, passes in a message object for them to handle
        public static void updateSubscribers(Message message)
        {
            //for each subscriber
            for (int i = 0; i < subscribers.Count; i++)
            {
                subscribers[i].Update(message);//send message
            }
        }

        //called when the app starts, runs in the background catching and publishing WS messages
        public static async Task HandleMessages()
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    while (ws.State == WebSocketState.Open)//while WS connection is active
                    {
                        WebSocketReceiveResult result;
                        do//get messages
                        {
                            var messageBuffer = WebSocket.CreateClientBuffer(1024, 16);
                            result = await ws.ReceiveAsync(messageBuffer, CancellationToken.None);
                            ms.Write(messageBuffer.Array, messageBuffer.Offset, result.Count);
                        }
                        while (!result.EndOfMessage);

                        if (result.MessageType == WebSocketMessageType.Text)// if message is a text message
                        {
                            var msgString = Encoding.UTF8.GetString(ms.ToArray());
                            var message = JsonConvert.DeserializeObject<Message>(msgString);//convert to message object

                            
                            updateSubscribers(message);//send to subscribers...
                        }

                        ms.Seek(0, SeekOrigin.Begin);
                        ms.Position = 0;
                        ms.SetLength(0);//clears the memory stream for a new message, otherwise it gets blocked up
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception is as follows: ");
                Console.WriteLine(Ex);

                await HandleMessages();
            }
        }

    }
}
