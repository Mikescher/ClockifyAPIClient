using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ClockifyClient.Model;
using Newtonsoft.Json;

namespace ClockifyClient
{
	class ClockifyAPIConnection
	{
		private const string BASE_PATH = "https://api.clockify.me/api/v1";

		private readonly string _apikey;

		private static readonly HttpClient _client = new HttpClient();

		public ClockifyAPIConnection(string apikey)
		{
			_apikey = apikey;
		}

		private async Task<XDocument> Query(HttpMethod method, string path)
		{
			var request = new HttpRequestMessage(method, BASE_PATH + path);

			request.Headers.Add("X-Api-Key", _apikey);

			var r = await _client.SendAsync(request);

			if (!r.IsSuccessStatusCode) throw new APIException($"Request failed: {path}: {r.StatusCode}");

			return JsonConvert.DeserializeXNode(await r.Content.ReadAsStringAsync(), "root");
		}

		public async Task<ClockifyUser> QueryCurrentUser()
		{
			var xdoc = (await Query(HttpMethod.Get, "/user")).Root;

			if (xdoc == null) throw new APIException("Deserialize failed (Root == null)");

			return new ClockifyUser(xdoc.Descendants("id").First().Value, "mail", "name");
		}
	}
}
