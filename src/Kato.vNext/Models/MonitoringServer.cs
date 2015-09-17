using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Kato.vNext.Models
{
    public class MonitoringServer : ObservableObject
    {
        private CancellationTokenSource _cts;
        public string WebsiteName { get; private set; }
        public string Url { get; private set; }

        public string Cookies { get; private set; }

        private bool _isUp;

        public bool IsUp
        {
            get { return _isUp; }
            set
            {
                if (Set(() => IsUp, ref _isUp, value))
                {
                    RaisePropertyChanged(() => Status);
                }
            }
        }

        public BuildStatus Status
        {
            get
            {
                if (IsUp)
                {
                    return BuildStatus.Success;
                }
                return BuildStatus.Failed;
            }
        }

        public MonitoringServer(string name, string url, string cookies = "")
        {
            WebsiteName = name;
            Url = url;
            Cookies = cookies;
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
                await CheckWebsite();
                _cts = null;
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }


        private async Task CheckWebsite()
        {
            var cookieJar = new CookieContainer();
            var cookies = Cookies.Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var cookie in cookies)
            {
                var c = cookie.Split('=');
                if (c.Length == 2)
                {
                    cookieJar.Add(new Cookie(c[0], c[1], "/", new Uri(Url, UriKind.Absolute).Host));
                }
            }
            var handler = new HttpClientHandler
            {
                CookieContainer = cookieJar,
                UseCookies = true,
                UseDefaultCredentials = false
            };
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var r = await client.GetAsync(Url);
            IsUp = r.IsSuccessStatusCode;
        }
    }
}