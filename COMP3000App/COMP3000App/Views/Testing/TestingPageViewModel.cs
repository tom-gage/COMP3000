﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net.WebSockets;
using System.Threading;
using System.Linq;
using System.ComponentModel;
using System.Text.Json;
using COMP3000App.Objects.TestObjects;

namespace COMP3000App.Views.Testing
{
    class TestingPageViewModel
    {
        //VARS

        public EateryOption eateryOption = new EateryOption("title", "description", 1.0f);

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

        public IList<EateryOption> testCollection { get; private set; }

        //CONSTRUCTOR
        public TestingPageViewModel()
        {
            testCollection = new List<EateryOption>();

            //testCollection.Add(new testData { title = "title 0", description = "description 0"});
            //testCollection.Add(new testData { title = "title 1", description = "description 1" });
            //testCollection.Add(new testData { title = "title 2", description = "description 2" });
            //testCollection.Add(new testData { title = "title 3", description = "description 3" });
            //testCollection.Add(new testData { title = "title 4", description = "description 4" });



            TestText = "this is the test Text";

            eateryOption = new EateryOption("title", "description", 1.1f);
            testCollection.Add(eateryOption);

            //connectToWS();
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

                    eateryOption = JsonSerializer.Deserialize<EateryOption>(TestText);

                } while (!result.EndOfMessage);
            }
        }
    }
}
