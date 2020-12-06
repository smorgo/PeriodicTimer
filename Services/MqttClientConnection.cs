using System;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using Smorgo.PeriodicTimer.Models;

namespace Smorgo.PeriodicTimer.Services
{
    public class MqttClientConnection
    {
        private string _broker;
        private string _username;
        private string _password;
        private string _topic;
        private IMqttClient _mqttClient;

        public MqttClientConnection(String broker, String username, String password, String topic)
        {
            _broker = broker;
            _username = username;
            _password = password;
            _topic = topic;
        }

        public void Connect()
        {
            Task.Run(() => ConnectAsync()).Wait();
        }

        private async Task ConnectAsync()
        {
            MqttNetConsoleLogger.ForwardToConsole();

            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var clientOptionsBuilder = new MqttClientOptionsBuilder()
                //.WithClientId()
                .WithTcpServer(_broker);

            if(!string.IsNullOrWhiteSpace(_username) || !string.IsNullOrWhiteSpace(_password))
            {
                clientOptionsBuilder = clientOptionsBuilder.WithCredentials(_username, _password);
            }

            var clientOptions = clientOptionsBuilder.Build();
            
            _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(async e =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER ###");
                await Task.Delay(0);
            });

            _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(async e =>
            {
                Console.WriteLine("### DISCONNECTED FROM SERVER ###");
                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                }
                catch
                {
                    Console.WriteLine("### RECONNECTING FAILED ###");
                }
            });

            try
            {
                await _mqttClient.ConnectAsync(clientOptions);
            }
            catch (Exception exception)
            {
                Console.WriteLine("### CONNECTING FAILED ###" + Environment.NewLine + exception);
            }
        }

        public void Publish(IMqttMessage message)
        {
            Task.Run(() => PublishAsync(message)).Wait();
        }
        private async Task PublishAsync(IMqttMessage message)
        {
            var payload = message.Payload;

            var applicationMessage = new MqttApplicationMessageBuilder()
                        .WithTopic(_topic)
                        .WithPayload(payload)
                        .WithAtLeastOnceQoS()
                        .Build();

            Console.WriteLine("### SENDING APPLICATION MESSAGE ###");
            Console.WriteLine($"+ Topic = {_topic}");
            Console.WriteLine($"+ Payload = {payload}");
            Console.WriteLine();

            await _mqttClient.PublishAsync(applicationMessage);
        }
    }
}