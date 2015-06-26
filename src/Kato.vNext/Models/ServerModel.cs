using System.Linq;
using Jenkins.Api.Client;

namespace Kato.vNext.Models
{
    public class ServerModel
    {
        public string Name { get; private set; }
        public string Url { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public int NbJobs { get; private set; }

        public ServerModel(string serverName, string url, string login, string password, int nbjobs)
        {
            Name = serverName;
            Url = url;
            Login = login;
            Password = password;
            NbJobs = nbjobs;
        }
    }
}