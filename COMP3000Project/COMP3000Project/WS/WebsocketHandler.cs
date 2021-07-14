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


    public static class WebsocketHandler //: Publisher//: INotifyPropertyChanged
    {
        //magic
        //public event PropertyChangedEventHandler PropertyChanged;
        //static public ObservableCollection<EateryOption> MessageList { get => _MessageList; set => SetProperty(ref _MessageList, value); }


        //VARIABLES
        //private ObservableCollection<EateryOption> _EateriesArray;
        //public ObservableCollection<EateryOption> EateriesArray { get => _EateriesArray; set => _EateriesArray = value; }

        static List<Subscriber> subscribers = new List<Subscriber>();

        static ClientWebSocket ws = new ClientWebSocket();


        //FUNCTIONS
        //initialises connection to the server
        public static async Task InitialiseConnectionAsync()
        {
            await ws.ConnectAsync(new Uri("ws://10.0.2.2:9000"), CancellationToken.None);

            await HandleMessages();

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

        public static async void RequestStartNewSearch(string currentUsername, string currentPassword, string locationName, TimeSpan desiredTime, string[] eateryTypes)
        {
            object[] messageItems = { currentUsername, currentPassword, locationName, desiredTime, eateryTypes };

            //make request message object
            Message request = new Message("1", "startNewSearch", "", messageItems);

            //send to server
            SendRequest(request);
        }

        public static async void RequestStartNewSearch(string currentUsername, string currentPassword, double lattitude, double longitude)
        {
            object[] messageItems = { currentUsername, currentPassword, lattitude, longitude };

            //make request message object
            Message request = new Message("1", "startNewSearch", "", messageItems);

            //send to server
            SendRequest(request);
        }

        public static async void RequestJoinExistingSearch(string currentUsername, string currentPassword, string searchCode)
        {
            object[] messageItems = { currentUsername, currentPassword, searchCode };

            //make request message object
            Message request = new Message("1", "joinExistingSearch", "", messageItems);

            //send to server
            SendRequest(request);
        }

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
        public static void registerSubscriber(Subscriber subscriber)
        {
            subscribers.Add(subscriber);
        }

        public static void removeSubsciber(Subscriber subscriber)
        {
            subscribers.Remove(subscriber);
        }

        public static void updateSubscribers(Message message)
        {
            for (int i = 0; i < subscribers.Count; i++)
            {
                subscribers[i].Update(message);
            }
        }

        //this bad boy handles all incoming messages
        //don't touch him
        public static async Task HandleMessages()
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    while (ws.State == WebSocketState.Open)
                    {
                        WebSocketReceiveResult result;
                        do
                        {
                            var messageBuffer = WebSocket.CreateClientBuffer(1024, 16);
                            result = await ws.ReceiveAsync(messageBuffer, CancellationToken.None);
                            ms.Write(messageBuffer.Array, messageBuffer.Offset, result.Count);
                        }
                        while (!result.EndOfMessage);

                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            var msgString = Encoding.UTF8.GetString(ms.ToArray());
                            var message = JsonConvert.DeserializeObject<Message>(msgString);

                            Console.WriteLine("[WS] Got a message of type " + message.type);
                            // Message was intended for us!
                            switch (message.type)
                            {
                                case "debugMessage":
                                    Console.WriteLine(message.Body);
                                    break;

                                case "eateryOptionsArray":

                                    updateSubscribers(message);

                                    break;

                                case "loginRequestGranted":

                                    //Console.WriteLine("[WS] got LoginRequestGranted !!");
                                    updateSubscribers(message);
                                    break;

                                case "usernameUpdated":

                                    //Console.WriteLine("[WS] got username updated !!");
                                    updateSubscribers(message);
                                    break;

                                case "passwordUpdated":

                                    //Console.WriteLine("[WS] got password updated !!");
                                    updateSubscribers(message);
                                    break;

                                case "userDeleted":

                                    //Console.WriteLine("[WS] got password updated !!");
                                    updateSubscribers(message);
                                    break;

                                case "newActiveSearchRequestGranted":
                                    updateSubscribers(message);
                                    break;

                                case "joinSearchRequestGranted":
                                    updateSubscribers(message);
                                    break;

                                case "gotMatch":
                                    updateSubscribers(message);
                                    break;

                                default:

                                    break;
                            }
                        }

                        ms.Seek(0, SeekOrigin.Begin);
                        ms.Position = 0;
                        ms.SetLength(0);//clears the memory stream for a new message, otherwise it gets blocked up
                    }
                }
            }
            //catch (InvalidOperationException)
            //{
            //    Console.WriteLine("[WS] Tried to receive message while already reading one.");
            //}
            catch (Exception Ex)
            {
                Console.WriteLine("Exception is as follows: ");
                Console.WriteLine(Ex);
            }
        }

        //BLACK MAGIC - THOU SHALT NOT TOUCH
        //void SetProperty<T>(ref T backingStore, T value, Action onChanged = null, [CallerMemberName] string propertyName = "")
        //{
        //    if (EqualityComparer<T>.Default.Equals(backingStore, value))
        //        return;

        //    backingStore = value;

        //    onChanged?.Invoke();

        //    HandlePropertyChanged(propertyName);
        //}

        //void HandlePropertyChanged(string propertyName = "") =>
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
