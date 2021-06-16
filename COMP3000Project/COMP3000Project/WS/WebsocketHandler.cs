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


    public class WebsocketHandler : Publisher//: INotifyPropertyChanged
    {
        //magic
        //public event PropertyChangedEventHandler PropertyChanged;
        //static public ObservableCollection<EateryOption> MessageList { get => _MessageList; set => SetProperty(ref _MessageList, value); }


        //VARIABLES
        private ObservableCollection<EateryOption> _EateriesArray;
        public ObservableCollection<EateryOption> EateriesArray { get => _EateriesArray; set => _EateriesArray = value; }

        List<Subscriber> subscribers = new List<Subscriber>();

        ClientWebSocket ws = new ClientWebSocket();

        public WebsocketHandler()
        {

        }

        //FUNCTIONS
        public async Task InitialiseConnectionAsync()
        {
            await ws.ConnectAsync(new Uri("ws://10.0.2.2:9000"), CancellationToken.None);

            await HandleMessages();

        }

        public async Task HandleMessages()
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
                                    Console.WriteLine("------ MESSAGE RECEIVED ------");
                                    Console.WriteLine(message.Body);
                                    break;

                                case "eatery options array":
                                    Console.WriteLine("[WS] Eatery options recieved");

                                    updateSubscribers(message.Body);

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
            catch (InvalidOperationException)
            {
                Console.WriteLine("[WS] Tried to receive message while already reading one.");
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception is as follows: ");
                Console.WriteLine(Ex);
            }
        }

        public async void SendMessage(string data)
        {
            var encodedData = Encoding.UTF8.GetBytes(data);
            var buffer = new ArraySegment<Byte>(encodedData, 0, encodedData.Length);
            await ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async void RequestJoinSearchAsync()
        {
            var data = "requestJoinSearch";

            var encodedData = Encoding.UTF8.GetBytes(data);
            var buffer = new ArraySegment<Byte>(encodedData, 0, encodedData.Length);
            await ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task<ObservableCollection<EateryOption>> RequestEateriesList(string data)
        {
            //send request
            var encodedData = Encoding.UTF8.GetBytes(data);
            var buffer = new ArraySegment<Byte>(encodedData, 0, encodedData.Length);
            await ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

            return null;
        }

        //Publisher / Subscriber stuff, handles inter-class communication
        public void registerSubscriber(Subscriber subscriber)
        {
            subscribers.Add(subscriber);
        }

        public void removeSubsciber(Subscriber subscriber)
        {
            subscribers.Remove(subscriber);
        }

        public void updateSubscribers(string jsonData)
        {
            foreach(Subscriber subscriber in subscribers)
            {
                subscriber.Update(jsonData);
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
