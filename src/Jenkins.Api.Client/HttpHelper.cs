using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jenkins.Api.Client
{
	public static class HttpHelper
	{
		public static Task<string> GetJsonAsync(Uri path)
		{
		    return GetJsonAsync(path, CancellationToken.None);
		}

        public static async Task<string> GetJsonAsync(Uri path, CancellationToken token)
        {
            using (HttpClient client = new HttpClient { BaseAddress = new Uri(path.Scheme + "://" + path.Host + ":" + path.Port) })
            {
                HttpResponseMessage result = await client.GetAsync(path.PathAndQuery, token);
                result.EnsureSuccessStatusCode();
                return await result.Content.ReadAsStringAsync();
            }
        }


        public static Task<string> GetJson(Uri path)
        {
            return GetJson(path, CancellationToken.None);
        }

        public static async Task<string> GetJson(Uri path, CancellationToken token)
        {
            using (HttpClient client = new HttpClient { BaseAddress = new Uri(path.Scheme + "://" + path.Host + ":" + path.Port) })
            {
                HttpResponseMessage result = await client.GetAsync(path.PathAndQuery, token);
                result.EnsureSuccessStatusCode();
                return await result.Content.ReadAsStringAsync();
            }
        }

        public static Task<ConsoleOutput> GetConsoleOutput(Uri path, long offset)
        {
            return GetConsoleOutput(path, offset, CancellationToken.None);
        }

        public static async Task<ConsoleOutput> GetConsoleOutput(Uri path, long offset , CancellationToken token)
        {
            using (HttpClient client = new HttpClient { BaseAddress = new Uri(path.Scheme + "://" + path.Host + ":" + path.Port) })
            {
                HttpResponseMessage result = await client.GetAsync(path.PathAndQuery + "logText/progressiveText?start=" + offset, token);
                result.EnsureSuccessStatusCode();

                long newOffset = int.Parse(result.Headers.GetValues("X-Text-Size").Single());

                IEnumerable<string> values;
                bool isBuilding = result.Headers.TryGetValues("X-More-Data", out values);

                return new ConsoleOutput { Text = await result.Content.ReadAsStringAsync(), Offset = newOffset, IsBuilding = isBuilding };
            }
        }

        public static Task<string> PostData(Uri path, string data = "")
        {
            return PostData(path, CancellationToken.None, data);
        }

        public static async Task<string> PostData(Uri path, CancellationToken token, string data = "")
        {
            using (HttpClient client = new HttpClient { BaseAddress = new Uri(path.Scheme + "://" + path.Host + ":" + path.Port) })
            {
                HttpResponseMessage result = await client.PostAsync(path.PathAndQuery, new StringContent(data), token);
                result.EnsureSuccessStatusCode();
                return await result.Content.ReadAsStringAsync();
            }
        }

        public static Task<T> GetObjectAsync<T>(string json) where T : class
		{
			if (string.IsNullOrWhiteSpace(json))
				return null;

			return Task.Run(() => JsonConvert.DeserializeObject<T>(json));
		}
    }
}
