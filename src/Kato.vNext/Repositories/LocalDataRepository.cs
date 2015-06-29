using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kato.vNext.Core;
using Kato.vNext.Models;

namespace Kato.vNext.Repositories
{
    public class LocalDataRepository
    {
        static readonly string SavedFilePathBase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Kato");
        public Task SaveDataAsync(List<ServerModel> servers)
        {
            var persistedData = new PersistedUserData(SavedFilePathBase);
            UserData data = new UserData() { Servers = new List<SavedJenkinsServers>() };
            foreach (ServerModel server in servers)
            {
                data.Servers.Add(new SavedJenkinsServers { Name = server.Name, Jobs = server.JobsSubscribed.Select(x => new SavedJob() { Name = x.Name, Url = x.Url }).ToList(), Url = server.Url, Login = server.Login, Password = server.Password });
            }
            return persistedData.SaveAsync(data);
        }

        public async Task<List<ServerModel>> RetrieveDataAsync()
        {
            var persistedData = new PersistedUserData(SavedFilePathBase);
            var data = await persistedData.OpenAsync<UserData>() ?? new UserData();
            var servers = new List<ServerModel>();
            foreach (var server in data.Servers)
            {
                var serverModel = new ServerModel(server.Name, server.Url, server.Login, server.Password, 0);
                serverModel.JobsSubscribed.AddRange(server.Jobs.Select(x => new JobModel(serverModel) { Name = x.Name, Url = x.Url, Subscribe = true, ServerName = server.Name}));
                servers.Add(serverModel);
            }
            return servers;
        }
    }
}
