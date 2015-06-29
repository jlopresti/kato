using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Jenkins.Api.Client;
using Kato.vNext.Helpers;

namespace Kato.vNext.Models
{
    public class ServerModel : ObservableObject
    {
        private JenkinsClient _client;
        private CancellationTokenSource _cts;

        public string Name { get; private set; }
        public string Url { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        private int _nbJobs;

        public int NbJobs
        {
            get { return _nbJobs; }
            set { Set(() => NbJobs, ref _nbJobs, value); }
        }

        public List<JobModel> JobsSubscribed { get; private set; }


        public ServerModel(string serverName, string url, string login, string password, int nbjobs)
        {
            Name = serverName;
            Url = url;
            Login = login;
            Password = password;
            NbJobs = nbjobs;
            _client = JenkinsClientFactory.CreateJenkinsClient(this);
            JobsSubscribed = new List<JobModel>();
        }

        public async Task RefreshAsync()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }

            try
            {
                _cts = new CancellationTokenSource();
                Server server = await _client.GetJson<Server>(new Uri(Url), _cts.Token);
                NbJobs = server.Jobs.Count();
                _cts = null;
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                
            }
        }
    }
}