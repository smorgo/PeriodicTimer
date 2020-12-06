using System;
using Smorgo.PeriodicTimer.Services;

namespace Smorgo.PeriodicTimer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PeriodicTimer Service Starting");

            var interval = Environment.GetEnvironmentVariable("TIMER_INTERVAL");
            var broker = Environment.GetEnvironmentVariable("MQTT_BROKER");
            var topic = Environment.GetEnvironmentVariable("MQTT_TOPIC");
            var username = Environment.GetEnvironmentVariable("MQTT_USER");
            var password = Environment.GetEnvironmentVariable("MQTT_PASSWORD");
            var messageLabel = Environment.GetEnvironmentVariable("MESSAGE_LABEL");

            var client = new MqttClientConnection(broker, username, password, topic);

            client.Connect();
            
            var timerLoop = new TimerLoop(client, interval, messageLabel);

            timerLoop.Run();

            Console.WriteLine("PeriodicTimer Service Ending");
        }
    }
}
