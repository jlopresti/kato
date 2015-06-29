using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Jenkins.Api.Client;
using Kato.vNext.Helpers;

namespace Kato.vNext.Models
{
    public class BuildModel : ObservableObject
    {
        private JenkinsClient _client;
        private CancellationTokenSource _cts;
        private ServerModel _server;
        private Build _build;
        public BuildModel(Build build)
        {
            _build = build;
            TimeStamp = ConvertTimestamp(_build.Timestamp);
            EstimatedDuration = TimeSpan.FromMilliseconds(_build.EstimatedDuration);
            Duration = _build.Duration == 0 && Building ? (DateTime.UtcNow - TimeStamp) : TimeSpan.FromMilliseconds(_build.Duration);
        }

        public bool Building { get { return _build.Building; } }
        public string Result { get { return _build.Result; } }
        public string BuiltOn { get { return _build.BuiltOn; } }
        public string Url { get { return _build.Url.ToString(); } }

        public double BuildPercentage
        {
            get { return Building ? (Duration.TotalSeconds / EstimatedDuration.TotalSeconds) * 100.00 : 0; }
        }

        public TimeSpan Duration { get; private set; }

        public TimeSpan EstimatedDuration { get; private set; }

        public DateTime TimeStamp { get; private set; }

        static readonly DateTime s_epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private DateTime ConvertTimestamp(long timeStamp)
        {
            return s_epoch + TimeSpan.FromMilliseconds(timeStamp);
        }

        private void EnsureClientInitialized()
        {
            if (!string.IsNullOrEmpty(Url) && _client == null)
                _client = JenkinsClientFactory.CreateJenkinsClient(_server);
        }

        public async Task LoadDetails(ServerModel server)
        {
            _server = server;

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

        private string _startedBy;

        public string StartedBy
        {
            get { return _startedBy; }
            set { Set(() => StartedBy, ref _startedBy, value); }
        }


        private async Task LoadJobDetail()
        {
            var details = await _client.GetJson<BuildDetails>(new Uri(Url), _cts.Token);
            if (details != null && details.actions != null)
            {
                var desc = details.actions.Where(x => x.causes != null).SelectMany(x => x.causes).FirstOrDefault(x => x != null && !string.IsNullOrEmpty(x.shortDescription));
                if (desc != null)
                {
                    StartedBy = desc.shortDescription;
                }
            }
        }
    }
}
