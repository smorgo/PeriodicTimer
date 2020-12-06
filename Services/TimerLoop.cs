using System;
using System.Threading.Tasks;
using Smorgo.PeriodicTimer.Models;

namespace Smorgo.PeriodicTimer.Services
{
    public class TimerLoop
    {
        private TimeSpan _interval;
        private String _messageLabel;
        private MqttClientConnection _client;

        public TimerLoop(MqttClientConnection client, string interval, string messageLabel)
        {
            _client = client;

            if(!TimeSpan.TryParse(interval, out _interval))
            {
                throw new ArgumentException($"Could not parse timer interval: {interval}", nameof(interval));
            }

            _messageLabel = messageLabel;
        }

        public void Run()
        {
            Task.Run(() => RunAsync()).Wait();
        }

        private async Task RunAsync()
        {
            while(true)
            {
                var message = new TimerMessage
                {
                    Label = _messageLabel,
                    Timestamp = DateTime.UtcNow
                };

                _client.Publish(message);

                await Task.Delay(_interval);
            }
        }
    }
}