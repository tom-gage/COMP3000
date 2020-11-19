using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using COMP3000Project.TestObjects;

namespace COMP3000Project._RemoteDataHandler
{
    class RemoteDataHandler
    {
        public async Task<EateryOption> connectToWS()
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

                    return JsonSerializer.Deserialize<EateryOption>(serialisedMessage);

                } while (!result.EndOfMessage);
            }
        }
    }
}
