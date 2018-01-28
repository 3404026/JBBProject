using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JBBService
{

    class WebSocketServer
    {
        public WebSocketServer()
        {
            Start();
        }

        public async void Start()
        {
            try
            {
                var listener = new HttpListener();
                listener.Prefixes.Add("http://syscotech.cc:1919/");
                listener.Start();

                while (true)
                {
                    var context = await listener.GetContextAsync();
                    Console.WriteLine("connected");

                    var websocketContext = await context.AcceptWebSocketAsync(null);
                    ProcessClient(websocketContext.WebSocket);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message.ToString());
            }



        }

        public async void ProcessClient(WebSocket websocket)
        {

            try
            {
                var data = new byte[1500];
                var buffer = new ArraySegment<byte>(data);

                while (true)
                {
                    var result = await websocket.ReceiveAsync(buffer, CancellationToken.None);

                    if (result.CloseStatus != null)
                    {
                        Console.WriteLine("socket closed");
                        websocket.Abort();
                        return;
                    }

                    Console.WriteLine(">>> " + Encoding.UTF8.GetString(data, 0, result.Count));
                    await websocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message.ToString());
            }

        }
    }


}
