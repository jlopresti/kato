using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jenkins.Api.Client
{
    public class HttpHelper
    {
        private readonly string _login;
        private readonly string _password;
        public HttpHelper() : this(string.Empty, string.Empty)
        {

        }
        public HttpHelper(string login, string password)
        {
            _login = login;
            _password = password;
        }

        public void SetBasicAuthHeader(HttpClient client)
        {
            if (!string.IsNullOrEmpty(_login) && !string.IsNullOrEmpty(_password))
            {
                string authInfo = _login + ":" + _password;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);
            }
        }
        public Task<string> GetJsonAsync(Uri path)
        {
            return GetJsonAsync(path, CancellationToken.None);
        }

        public async Task<string> GetJsonAsync(Uri path, CancellationToken token)
        {
            using (HttpClient client = new HttpClient { BaseAddress = new Uri(path.Scheme + "://" + path.Host + ":" + path.Port) })
            {
                SetBasicAuthHeader(client);
                HttpResponseMessage result = await client.GetAsync(path.PathAndQuery, token);
                result.EnsureSuccessStatusCode();
                return await result.Content.ReadAsStringAsync();
            }
        }


        public Task<string> GetJson(Uri path)
        {
            return GetJson(path, CancellationToken.None);
        }

        public async Task<string> GetJson(Uri path, CancellationToken token)
        {
            using (HttpClient client = new HttpClient { BaseAddress = new Uri(path.Scheme + "://" + path.Host + ":" + path.Port) })
            {
                SetBasicAuthHeader(client);
                HttpResponseMessage result = await client.GetAsync(path.PathAndQuery, token);
                result.EnsureSuccessStatusCode();
                return await result.Content.ReadAsStringAsync();
            }
        }

        public Task<ConsoleOutput> GetConsoleOutput(Uri path, long offset)
        {
            return GetConsoleOutput(path, offset, CancellationToken.None);
        }

        public async Task<ConsoleOutput> GetConsoleOutput(Uri path, long offset, CancellationToken token)
        {
            using (HttpClient client = new HttpClient { BaseAddress = new Uri(path.Scheme + "://" + path.Host + ":" + path.Port) })
            {
                SetBasicAuthHeader(client);
                HttpResponseMessage result = await client.GetAsync(path.PathAndQuery + "logText/progressiveText?start=" + offset, token);
                result.EnsureSuccessStatusCode();

                long newOffset = int.Parse(result.Headers.GetValues("X-Text-Size").Single());

                IEnumerable<string> values;
                bool isBuilding = result.Headers.TryGetValues("X-More-Data", out values);

                return new ConsoleOutput { Text = await result.Content.ReadAsStringAsync(), Offset = newOffset, IsBuilding = isBuilding };
            }
        }

        public Task<string> PostData(Uri path, string data = "")
        {
            return PostData(path, CancellationToken.None, data);
        }

        public async Task<string> PostData(Uri path, CancellationToken token, string data = "")
        {
            using (HttpClient client = new HttpClient { BaseAddress = new Uri(path.Scheme + "://" + path.Host + ":" + path.Port) })
            {
                SetBasicAuthHeader(client);
                HttpResponseMessage result = await client.PostAsync(path.PathAndQuery, new StringContent(data), token);
                result.EnsureSuccessStatusCode();
                return await result.Content.ReadAsStringAsync();
            }
        }

        public Task<T> GetObjectAsync<T>(string json) where T : class
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            return Task.Run(() => JsonConvert.DeserializeObject<T>(json));
        }
    }
}
