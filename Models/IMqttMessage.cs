using System;
namespace Smorgo.PeriodicTimer.Models
{
    public interface IMqttMessage
    {
        String Payload {get;}
    }
}