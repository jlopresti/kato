using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jenkins.Api.Client
{
	public static class HttpHelper
	{
		public static async Task<string> GetJsonAsync(Uri path)
		{
			using (HttpClient client = new HttpClient { BaseAddress = new Uri(path.Scheme + "://" + path.Host + ":" + path.Port) })
			{
				HttpResponseMessage result = await client.GetAsync(path.PathAndQuery);
				result.EnsureSuccessStatusCode();
				return await result.Content.ReadAsStringAsync();
			}
		}

		public static async Task<string> GetJson(Uri path)
		{
			using (HttpClient client = new HttpClient { BaseAddress = new Uri(path.Scheme + "://" + path.Host + ":" + path.Port) })
			{
				HttpResponseMessage result = await client.GetAsync(path.PathAndQuery);
				result.EnsureSuccessStatusCode();
				return await result.Content.ReadAsStringAsync();
			}
		}

		public static async Task<ConsoleOutput> GetConsoleOutput(Uri path, long offset)
		{
			using (HttpClient client = new HttpClient { BaseAddress = new Uri(path.Scheme + "://" + path.Host + ":" + path.Port) })
			{
				HttpResponseMessage result = await client.GetAsync(path.PathAndQuery + "logText/progressiveText?start=" + offset);
				result.EnsureSuccessStatusCode();

				long newOffset = int.Parse(result.Headers.GetValues("X-Text-Size").Single());

				IEnumerable<string> values;
				bool isBuilding = result.Headers.TryGetValues("X-More-Data", out values);

				return new ConsoleOutput { Text = await result.Content.ReadAsStringAsync(), Offset = newOffset, IsBuilding = isBuilding };
			}
		}

		public static async Task<string> PostData(Uri path, string data = "")
		{
			using (HttpClient client = new HttpClient { BaseAddress = new Uri(path.Scheme + "://" + path.Host + ":" + path.Port) })
			{
				HttpResponseMessage result = await client.PostAsync(path.PathAndQuery, new StringContent(data));
				result.EnsureSuccessStatusCode();
				return await result.Content.ReadAsStringAsync();
			}
		}

		public static T GetObject<T>(string json) where T : class
		{
			if (string.IsNullOrWhiteSpace(json))
				return null;

			return JsonConvert.DeserializeObject<T>(json);
		}
	}
}
