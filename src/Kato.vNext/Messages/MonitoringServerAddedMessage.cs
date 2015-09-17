using Kato.vNext.Models;

namespace Kato.vNext.Messages
{
    public class MonitoringServerAddedMessage
    {
        public MonitoringServer Website { get; private set; }
        public MonitoringServerAddedMessage(MonitoringServer website)
        {
            Website = website;
        }
    }
}