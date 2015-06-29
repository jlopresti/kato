using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jenkins.Api.Client;
using Kato.vNext.Models;

namespace Kato.vNext.Helpers
{
    public class JenkinsClientFactory
    {
        public static JenkinsClient CreateJenkinsClient(string url)
        {
            return new JenkinsClient(new Uri(url, UriKind.Absolute));
        }
        public static JenkinsClient CreateJenkinsClient(string url, string login, string password)
        {
            var client = new JenkinsClient(new Uri(url, UriKind.Absolute));
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
            {
                client.SetCredential(login, password);
            }
            return client;
        }

        public static JenkinsClient CreateJenkinsClient(ServerModel server)
        {
            if (server != null)
            {
                var client = new JenkinsClient(new Uri(server.Url, UriKind.Absolute));
                if (!string.IsNullOrEmpty(server.Login) && !string.IsNullOrEmpty(server.Password))
                {
                    client.SetCredential(server.Login, server.Password);
                }
                return client;
            }
            throw new Exception("Server not found");
        }
    }
}
