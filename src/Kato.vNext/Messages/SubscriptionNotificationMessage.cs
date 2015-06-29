using System.Collections.Generic;
using Kato.vNext.Models;

namespace Kato.vNext.Messages
{
    internal class SubscriptionNotificationMessage
    {
        public IEnumerable<JobModel> Jobs { get; private set; }

        public SubscriptionNotificationMessage(IEnumerable<JobModel> jobs)
        {
            Jobs = jobs;
        }
    }
}