using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Jenkins.Api.Client;
using Kato.vNext.Helpers;

namespace Kato.vNext.Models
{
    public class JobModel : ObservableObject
    {
        public delegate void StatusChangedEvent(object sender, StatusChangedArgs args);

        public event StatusChangedEvent StatusChanged;

        private bool _subscribe;
        private JenkinsClient _client;
        private CancellationTokenSource _cts;
        private BuildModel _lastBuild;
        private string _jobColor;
        private ServerModel _server;

        public string Name { get; set; }
        public string Url { get; set; }
        public string ServerName { get; set; }
        public Job JobDetail { get; set; }


        public string JobColor
        {
            get { return _jobColor; }
            set
            {
                if (_jobColor != value && value != null)
                {
                    BuildStatus oldStatus = Status;
                    _jobColor = value;
                    RaisePropertyChanged(() => JobColor);
                    RaisePropertyChanged(() => Status);
                    RaiseStatusChangedEvent(oldStatus, Status);
                }
            }
        }

        public bool Subscribe
        {
            get { return _subscribe; }
            set { Set(() => Subscribe, ref _subscribe, value); }
        }

        public BuildStatus Status
        {
            get
            {
                BuildStatus status;
                switch (JobColor)
                {
                    case "blue":
                        status = BuildStatus.Success;
                        break;
                    case "blue_anime":
                        status = BuildStatus.SuccessAndBuilding;
                        break;
                    case "red":
                        status = BuildStatus.Failed;
                        break;
                    case "red_anime":
                        status = BuildStatus.FailedAndBuilding;
                        break;
                    case "aborted":
                        status = BuildStatus.Aborted;
                        break;
                    case "aborted_anime":
                        status = BuildStatus.AbortedAndBuilding;
                        break;
                    case "disabled":
                        status = BuildStatus.Disabled;
                        break;
                    default:
                        status = BuildStatus.Unknown;
                        break;
                }

                return status;
            }
        }

        public BuildModel LastBuild
        {
            get { return _lastBuild; }
            set { Set(() => LastBuild, ref _lastBuild, value); }
        }


        public async Task RefreshAsync()
        {
            EnsureClientInitialized();

            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }

            try
            {
                _cts = new CancellationTokenSource();
                await LoadJobDetail();
                _cts = null;
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        private async Task LoadJobDetail()
        {
            JobDetail = await _client.GetJson<Job>(new Uri(Url), _cts.Token);
            JobColor = JobDetail.Color;
            LastBuild = new BuildModel(JobDetail.LastBuild);
            await LastBuild.LoadDetails(_server);
        }

        private void EnsureClientInitialized()
        {
            if (!string.IsNullOrEmpty(Url) && _client == null)
                _client = JenkinsClientFactory.CreateJenkinsClient(_server);
        }


        public void GoToLastBuildView()
        {
            Uri path = new Uri((JobDetail.LastBuild != null) ? JobDetail.LastBuild.Url.ToString(): Url);
            Process.Start(path.OriginalString);
        }

        public JobModel(ServerModel server)
        {
            _server = server;
        }

        private void RaiseStatusChangedEvent(BuildStatus oldValue, BuildStatus newValue)
        {
            if (StatusChanged != null)
                StatusChanged(this, new StatusChangedArgs(oldValue, newValue));
        }
    }
}