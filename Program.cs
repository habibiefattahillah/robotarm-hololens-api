using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace frrjiftest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frm());
        }
    }
    public class WebSocketServer
    {
        private static ConcurrentBag<WebSocket> _clients = new ConcurrentBag<WebSocket>();
        public static event Action<string> OnClientConnected;
        public static event Action<ReceivedData> OnDataReceived;

        public static async Task StartServerAsync(string api, string port)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($"http://{api}:{port}/");
            listener.Start();
            MessageBox.Show($"WebSocket Server started at ws://{api}:{port}/");

            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                    WebSocket clientSocket = webSocketContext.WebSocket;

                    // Add client to the list
                    _clients.Add(clientSocket);

                    MessageBox.Show($"New client connected. Total clients: {_clients.Count}");
                    Console.WriteLine($"New client connected. Total clients: {_clients.Count}");

                    // Handle the client in a separate task
                    _ = Task.Run(() => HandleConnectionAsync(clientSocket));
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }

        private static async Task HandleConnectionAsync(WebSocket socket)
        {
            byte[] buffer = new byte[1024];
            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        MessageBox.Show($"Received from client: {message}");
                        Console.WriteLine($"Received from client: {message}");

                        // Attempt to parse the received message into the ReceivedData object
                        try
                        {
                            var data = JsonConvert.DeserializeObject<ReceivedData>(message);

                            if (data != null)
                            {
                                Console.WriteLine($"Parsed Data: x = {data.x}, y = {data.y}, z = {data.z}, w = {data.w}, p = {data.p}, r = {data.r}");

                                // Example: Broadcast the parsed data to all clients
                                string responseMessage = $"Broadcast: x = {data.x}, y = {data.y}, z = {data.z}, w = {data.w}, p = {data.p}, r = {data.r}";

                                foreach (var client in _clients)
                                {
                                    if (client.State == WebSocketState.Open)
                                    {
                                        byte[] response = Encoding.UTF8.GetBytes(message);
                                        await client.SendAsync(new ArraySegment<byte>(response), WebSocketMessageType.Text, true, CancellationToken.None);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Received data is not in the expected format.");
                            }
                        }
                        catch (JsonException jsonEx)
                        {
                            Console.WriteLine($"Error deserializing message: {jsonEx.Message}");
                            // Optionally, send an error message back to the client
                            string errorMessage = "Invalid data format received.";
                            byte[] errorBuffer = Encoding.UTF8.GetBytes(errorMessage);
                            await socket.SendAsync(new ArraySegment<byte>(errorBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                        Console.WriteLine("Client disconnected.");
                        _clients.TryTake(out _); // Remove client from the list
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                _clients.TryTake(out _); // Ensure client is removed on error
            }
        }

        // Send data
        public static async Task SendDataAsync(WebSocket socket, string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public class ReceivedData
        {
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }
            public double w { get; set; }
            public double p { get; set; }
            public double r { get; set; }
        }

    }
}
