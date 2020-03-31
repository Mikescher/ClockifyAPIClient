using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using ClockifyAPIClient.Model;
using Newtonsoft.Json.Linq;

namespace ClockifyAPIClient
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

        private async Task<JToken> Query(HttpMethod method, string path, IReadOnlyCollection<(string, string)> urlparams = null, int retry = 0)
        {
            if (urlparams == null) urlparams = new (string, string)[0];

            var uri = BASE_PATH + path;
            if (urlparams.Count > 0) uri += "?" + string.Join("&", urlparams.Select(p => $"{p.Item1}={p.Item2}"));

            var request = new HttpRequestMessage(method, uri);

            request.Headers.Add("X-Api-Key", _apikey);

            var r = await _client.SendAsync(request);

            if (retry == 0 && r.StatusCode == HttpStatusCode.TooManyRequests)
            {
                await Task.Delay(1000);
                return await Query(method, path, urlparams, retry + 1);
            }
            else if (retry == 1 && r.StatusCode == HttpStatusCode.TooManyRequests)
            {
                await Task.Delay(5000);
                return await Query(method, path, urlparams, retry + 1);
            }


            if (!r.IsSuccessStatusCode) throw new APIException($"Request failed: {path}: {r.StatusCode}");

            return JToken.Parse(await r.Content.ReadAsStringAsync());
        }

        private async Task<List<JObject>> QueryPaginated(HttpMethod method, string path, IReadOnlyCollection<(string, string)> urlparams = null)
        {
            if (urlparams == null) urlparams = new (string, string)[0];

            var result = new List<JObject>();

            for (var ipage = 1; ; ipage++)
            {
                var json = await Query(method, path, urlparams.Concat(Enumerable.Repeat(("page", $"{ipage}"), 1)).ToList());

                if (json.Value<JArray>().Count == 0) return result;

                result.AddRange(json.Value<JArray>().Children<JObject>());
            }
        }

        public async Task<ClockifyUser> QueryCurrentUser()
        {
            var json = await Query(HttpMethod.Get, "/user");

            return new ClockifyUser(json.Value<string>("id"), json.Value<string>("email"), json.Value<string>("name"), null);
        }

        public async Task<List<ClockifyWorkspace>> QueryWorkspaces()
        {
            var json = await Query(HttpMethod.Get, "/workspaces");

            return json
                .Value<JArray>()
                .Children<JObject>()
                .Select(p => new ClockifyWorkspace(p.Value<string>("id"), p.Value<string>("name")))
                .ToList();
        }

        public async Task<List<ClockifyClient>> QueryWorkspaceClients(ClockifyWorkspace workspace)
        {
            var json = await QueryPaginated(HttpMethod.Get, $"/workspaces/{workspace.ID}/clients");

            return json
                .Select(p => new ClockifyClient(p.Value<string>("id"), p.Value<string>("name"), p.Value<bool>("archived"), workspace))
                .ToList();
        }

        public async Task<List<ClockifyUser>> QueryWorkspaceUsers(ClockifyWorkspace workspace)
        {
            var json = await QueryPaginated(HttpMethod.Get, $"/workspaces/{workspace.ID}/users");

            return json
                .Select(p => new ClockifyUser(p.Value<string>("id"), p.Value<string>("email"), p.Value<string>("name"), workspace))
                .ToList();
        }

        public async Task<List<ClockifyProject>> QueryWorkspaceProjects(ClockifyWorkspace workspace, List<ClockifyClient> clients)
        {
            var json = await QueryPaginated(HttpMethod.Get, $"/workspaces/{workspace.ID}/projects");

            return json
                .Select(p => new ClockifyProject(
                    p.Value<string>("id"), 
                    p.Value<string>("name"), 
                    p.Value<bool>("archived"), 
                    workspace, 
                    string.IsNullOrEmpty(p.Value<string>("clientId")) ? null : clients.SingleOrDefault(c => c.ID == p.Value<string>("clientId")) ?? throw new APIException($"Client with id '{p.Value<string>("clientId")}' not found")))
                .ToList();
        }

        public async Task<List<ClockifyTask>> QueryProjectTasks(ClockifyProject project)
        {
            var json = await QueryPaginated(HttpMethod.Get, $"/workspaces/{project.Workspace.ID}/projects/{project.ID}/tasks");

            return json
                .Select(p => new ClockifyTask(p.Value<string>("id"), p.Value<string>("name"), project, project.Workspace))
                .ToList();
        }

        public async Task<List<ClockifyTimeEntry>> QueryUserTimeEntries(ClockifyUser user, ClockifyWorkspace ws, List<ClockifyProject> projects, List<ClockifyTask> tasks)
        {
            var json = await QueryPaginated(HttpMethod.Get, $"/workspaces/{ws.ID}/user/{user.ID}/time-entries");

            return json
                .Select(p =>
                {
                    var proj = string.IsNullOrEmpty(p.Value<string>("projectId")) ? null : projects.SingleOrDefault(c => c.ID == p.Value<string>("projectId")) ?? throw new APIException($"Project with id '{p.Value<string>("projectId")}' not found");
                    var task = string.IsNullOrEmpty(p.Value<string>("taskId"))    ? null : tasks.SingleOrDefault(c    => c.ID == p.Value<string>("taskId"))    ?? throw new APIException($"Task with id '{   p.Value<string>("taskId")   }' not found");

                    var start = DateTime.Parse(p.Value<JObject>("timeInterval").Value<string>("start"));
                    var end   = DateTime.Parse(p.Value<JObject>("timeInterval").Value<string>("end"));
                    var span  = XmlConvert.ToTimeSpan(p.Value<JObject>("timeInterval").Value<string>("duration"));

                    return new ClockifyTimeEntry(p.Value<string>("id"), p.Value<string>("description"), p.Value<bool>("billable"), user, ws, proj, task, start, end, span);
                })
                .ToList();
        }
    }
}
