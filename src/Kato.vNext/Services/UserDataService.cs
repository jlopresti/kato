using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kato.vNext.Core;
using Kato.vNext.Models;

namespace Kato.vNext.Services
{
    public class UserDataService
    {
        static readonly string SavedFilePathBase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Kato");
        public void SaveData(List<ServerModel> servers)
        {
            var persistedData = new PersistedUserData(SavedFilePathBase);
            UserData data = new UserData() { Servers = new List<SavedJenkinsServers>() };
            foreach (ServerModel server in servers)
            {
                data.Servers.Add(new SavedJenkinsServers { Name = server.Name, Jobs = new List<SavedJob>(), Url = server.Url, Login = server.Login, Password = server.Password});
            }
            persistedData.Save(data);
        }

        public List<ServerModel> RetrieveServers()
        {
            var persistedData = new PersistedUserData(SavedFilePathBase);
            var data = persistedData.Open<UserData>() ?? new UserData();
            var servers = new List<ServerModel>();
            foreach (var server in data.Servers)
            {
                servers.Add(new ServerModel(server.Name, server.Url, server.Login, server.Password, 0));
            }
            return servers;
        }
    }
}
