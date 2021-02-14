using COMP3000Project.TestObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace COMP3000Project.WS
{


    class WebsocketHandler : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<EateryOption> _MessageList;
        public ObservableCollection<EateryOption> MessageList { get => _MessageList; set => SetProperty(ref _MessageList, value); }



        ClientWebSocket ws = new ClientWebSocket();

        public async Task InitialiseConnectionAsync()
        {
            await ws.ConnectAsync(new Uri("ws://10.0.2.2:9000"), CancellationToken.None);




            //while (true)//this gets messages, maybe
            //{
            //    WebSocketReceiveResult result;
            //    var message = new ArraySegment<byte>(new byte[4096]);
            //    do
            //    {
            //        result = await ws.ReceiveAsync(message, CancellationToken.None);
            //        var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
            //        string serialisedMessage = Encoding.UTF8.GetString(messageBytes);

            //        var textMessage = serialisedMessage;

            //        MessageList = JsonSerializer.Deserialize<ObservableCollection<EateryOption>>(textMessage);
            //        Console.WriteLine("");

            //    } while (!result.EndOfMessage);
            //}
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

            //get response?
            while (true)//this gets messages, maybe
            {
                WebSocketReceiveResult result;
                //var message = new ArraySegment<byte>(new byte[4096]);
                var message = new ArraySegment<byte>(new byte[10000]);//byte limit governs max size of message, ive slapped it up to high, should revisit it
                do
                {
                    result = await ws.ReceiveAsync(message, CancellationToken.None);
                    var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                    string serialisedMessage = Encoding.UTF8.GetString(messageBytes);

                    var textMessage = serialisedMessage;

                    return JsonSerializer.Deserialize<ObservableCollection<EateryOption>>(textMessage);
                    
                } while (!result.EndOfMessage);
            }
        }

        public async Task SendMessage(string data)
        {
            var encodedData = Encoding.UTF8.GetBytes(data);
            var buffer = new ArraySegment<Byte>(encodedData, 0, encodedData.Length);
            await ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        //BLACK MAGIC - THOU SHALT NOT TOUCH
        void SetProperty<T>(ref T backingStore, T value, Action onChanged = null, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;

            onChanged?.Invoke();

            HandlePropertyChanged(propertyName);
        }

        void HandlePropertyChanged(string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
