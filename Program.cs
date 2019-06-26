using System;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace mock_pinball
{
    class Program
    {
        private static DeviceClient deviceClient;

        // The device connection string to authenticate the device with your IoT hub.
        // Using the Azure CLI:
        // az iot hub device-identity show-connection-string --hub-name {YourIoTHubName} --device-id MyDotnetDevice --output table
        private readonly static string connectionString = "HostName=YOUR_IOT_HUB.azure-devices.net;DeviceId=YOUR_DEVICE_ID;SharedAccessKey=YOUR_SAS_KEY";

        static void Main(string[] args)
        {
            var playingAsComputer = (args.Length != 0) ? args[0] : "on";

            Console.WriteLine("Mock Pinballl IoT Hub Client - Simulated device.");
            Console.WriteLine($"AI will marked as '{playingAsComputer}'. Ctrl-C to exit.");

            // Connect to the IoT hub using the MQTT protocol
            deviceClient = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);

            SendDeviceToCloudMessagesAsync(playingAsComputer);
            Console.ReadLine();
        }

        // Async method to send simulated telemetry
        private static async void SendDeviceToCloudMessagesAsync(string playingAsComputer)
        {
            Random rand = new Random();
            var playChoice = 0;
            var nextPlayPause = 0;

            while (true)
            {
                // select next table response
                playChoice = rand.Next(0,5);
                // how long to wait before next table response
                nextPlayPause = rand.Next(1,5);

                string messageContent = string.Empty;

                Message message = null;

                switch(playChoice)
                {
                    // make balldrain 1/3 likely rather than 1/5
                    case 0:
                    case 5:
                        var ballDrain = new { eventtype = "balldrain", timestamp = DateTime.UtcNow };
                        messageContent =  JsonConvert.SerializeObject(ballDrain);                             
                        break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        var gameData = new { eventtype = "gamedata", scoretier = playChoice, aistate = playingAsComputer, timestamp = DateTime.UtcNow };
                        messageContent =  JsonConvert.SerializeObject(gameData);       
                        break;
                }
                
                message = new Message(Encoding.ASCII.GetBytes(messageContent));
                // Send the tlemetry message
                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.UtcNow, messageContent);

                // wait for next table response
                await Task.Delay(nextPlayPause * 1000);
            }
        }
    }
}