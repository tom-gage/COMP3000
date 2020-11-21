using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Net.WebSockets;
using System.Threading;
using System.Linq;
using System.ComponentModel;
using System.Text.Json;
using COMP3000Project.TestObjects;
using COMP3000Project._RemoteDataHandler;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace COMP3000Project.Views.Testing
{
    class TestingPageViewModel : INotifyPropertyChanged
    {
        //VARS

        public RemoteDataHandler remoteDataHandler;

        public EateryOption eateryOption;
        public class testData
        {
            public string title { get; set; }
            public string description { get; set; }
        }

        string testText;
        public event PropertyChangedEventHandler PropertyChanged;
        public string TestText
        {
            set
            {
                if (testText != value)
                {
                    testText = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("TestText"));
                    }
                }
            }
            get
            {
                return testText;
            }
        }
        ObservableCollection<EateryOption> _testCollection;
        public ObservableCollection<EateryOption> TestCollection { get => _testCollection; set => SetProperty(ref _testCollection, value); }

        //CONSTRUCTOR
        public TestingPageViewModel()
        {
            TestCollection = new ObservableCollection<EateryOption>();
            //eateryOption = new EateryOption("title", "description", 1.2f);
            //TestCollection.Add(eateryOption);
            connectToWS();


        }

        //FUNCTIONS
        public async void connectToWS()
        {
            ClientWebSocket ws = new ClientWebSocket();

            await ws.ConnectAsync(new Uri("ws://10.0.2.2:9000"), CancellationToken.None);

            var data = "please send me some restaurant data";

            var encodedData = Encoding.UTF8.GetBytes(data);
            var buffer = new ArraySegment<Byte>(encodedData, 0, encodedData.Length);
            await ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);


            while (true)
            {
                WebSocketReceiveResult result;
                var message = new ArraySegment<byte>(new byte[4096]);
                do
                {
                    result = await ws.ReceiveAsync(message, CancellationToken.None);
                    var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                    string serialisedMessage = Encoding.UTF8.GetString(messageBytes);

                    TestText = serialisedMessage;

                    TestCollection = JsonSerializer.Deserialize<ObservableCollection<EateryOption>>(TestText);

                    //TestCollection.Add(eateryOption);

                } while (!result.EndOfMessage);
            }
        }

        //BLACK MAGIC - THOU SHALT NOT TOUCH
        protected void SetProperty<T>(ref T backingStore, T value, Action onChanged = null, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;

            onChanged?.Invoke();

            HandlePropertyChanged(propertyName);
        }

        protected void HandlePropertyChanged(string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
