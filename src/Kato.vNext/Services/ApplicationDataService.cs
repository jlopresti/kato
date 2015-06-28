using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jenkins.Api.Client;
using Kato.vNext.Models;
using Kato.vNext.Repositories;

namespace Kato.vNext.Services
{
    public class ApplicationDataService
    {
        private LocalDataRepository _localDataRepository;
        private bool _isInitialize;
        private List<ServerModel> _savedServers;
        public ApplicationDataService()
        {
            _localDataRepository = new LocalDataRepository();
        }

        private async Task EnsureInitialize()
        {
            if (!_isInitialize)
            {
                await Initialize();
                _isInitialize = true;
            }
        }

        private async Task Initialize()
        {
            _savedServers = await _localDataRepository.RetrieveDataAsync();
        }

        public Task AddServerAsync(ServerModel server)
        {
            _savedServers.Add(server);
            return _localDataRepository.SaveDataAsync(_savedServers);
        }

        public async Task<List<ServerModel>> GetServersAsync()
        {
            await EnsureInitialize();
            return _savedServers;
        }

        public Task ClearDataAsync()
        {
            _savedServers = new List<ServerModel>();
            return _localDataRepository.SaveDataAsync(_savedServers);
        }


        private CancellationTokenSource _cts;
        public async Task<List<JobModel>> GetJobsAsync()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }

            await EnsureInitialize();
            var results = new List<JobModel>();
            try
            {
                _cts = new CancellationTokenSource();

                var works = new List<Task>();
                foreach (var savedServer in _savedServers)
                {
                    JenkinsClient client = new JenkinsClient(new Uri(savedServer.Url, UriKind.Absolute));
                    works.Add(client.GetJson<Server>(new Uri(savedServer.Url), _cts.Token)
                        .ContinueWith((x) => results.AddRange(x.Result.Jobs.Select(y =>
                        new JobModel()
                        {
                            Name = y.Name,
                            Url = y.Url.ToString(),
                            ServerName = savedServer.Name,
                            JobColor = y.Color,
                            Subscribe = savedServer.JobsSubscribed.Any(z => z.Url == y.Url.ToString() && z.Name == y.Name)
                        }))));
                }
                await Task.WhenAll(works);
                _cts = null;

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return results.Where(x => x != null).OrderBy(x => x.ServerName).ThenBy(x => x.Name).ToList();
        }

        public async Task<bool> AddSubscribe(JobModel job)
        {
            var server = _savedServers.FirstOrDefault(x => x.Name == job.ServerName);
            if (server != null)
            {
                server.JobsSubscribed.Add(job);
                await _localDataRepository.SaveDataAsync(_savedServers);
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveSubscribe(JobModel job)
        {
            var server = _savedServers.FirstOrDefault(x => x.Name == job.ServerName);
            if (server != null)
            {
                var jobToDelete = server.JobsSubscribed.FirstOrDefault(x => x.Name == job.Name && x.Url == job.Url);
                server.JobsSubscribed.Remove(jobToDelete);
                await _localDataRepository.SaveDataAsync(_savedServers);
                return true;
            }
            return false;
        }

        public async Task<List<JobModel>> GetSubscribedJobs()
        {
            await EnsureInitialize();
            return _savedServers.SelectMany(x => x.JobsSubscribed).ToList();
        }
    }
}
