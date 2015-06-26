using System.Security.AccessControl;
using Jenkins.Api.Client;
using Kato.vNext.Models;

namespace Kato.vNext.Messages
{
    internal class ServerAddedMessage
    {
        public ServerModel Server { get; private set; }
        
        public ServerAddedMessage(ServerModel server)
        {
            Server = server;
        }
    }
}