using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClockifyAPIClient.Model;

namespace ClockifyAPIClient
{
    static class Program
	{
        private static async Task Main(string[] args)
		{
			try
			{
				await Run(args);

                Console.WriteLine();
				Console.WriteLine("Finished");
                Console.WriteLine();
				//Console.ReadLine();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				//Console.ReadLine();
			}
		}

		private static async Task Run(string[] args)
		{
			if (args.Length != 2) throw new Exception("Correct Usage: \"clockifyclient <apikey> <filepath>\"");

			var arg_apikey   = args[0];
			var arg_filepath = args[1];

			arg_filepath = arg_filepath.Replace("{now6}", $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}");
			arg_filepath = arg_filepath.Replace("{now5}", $"{DateTime.Now:yyyy-MM-dd_HH-mm}");
			arg_filepath = arg_filepath.Replace("{now4}", $"{DateTime.Now:yyyy-MM-dd_HH}");
			arg_filepath = arg_filepath.Replace("{now3}", $"{DateTime.Now:yyyy-MM-dd}");
			arg_filepath = arg_filepath.Replace("{now2}", $"{DateTime.Now:yyyy-MM}");
			arg_filepath = arg_filepath.Replace("{now1}", $"{DateTime.Now:yyyy}");

            var api = new ClockifyAPIConnection(arg_apikey);
			var sql = new SqliteOutputWriter(arg_filepath);

            var currentuser = await api.QueryCurrentUser();
			Console.WriteLine($"Current user: {currentuser.Name}    {currentuser.Email}    ( {currentuser.ID} )");
			Console.WriteLine();


			var workspaces = await api.QueryWorkspaces();
			Console.WriteLine($"Found {workspaces.Count} Workspaces:");
			foreach (var wspc in workspaces) Console.WriteLine($"  - {wspc.Name,-16} ( {wspc.ID} )");
			Console.WriteLine();


			var users = new List<ClockifyUser>();
			foreach (var ws in workspaces) users.AddRange(await api.QueryWorkspaceUsers(ws));
			users = users.GroupBy(u => u.ID).Select(p => ClockifyUser.Merge(p.ToList())).ToList();
			Console.WriteLine($"Found {users.Count} Users:");
			foreach (var user in users) Console.WriteLine($"  - {user.Name,-16} {user.Email,-40} ( {user.ID} )");
			Console.WriteLine();


			var clients = new List<ClockifyClient>();
			foreach (var ws in workspaces) clients.AddRange(await api.QueryWorkspaceClients(ws));
			Console.WriteLine($"Found {clients.Count} Clients:");
			foreach (var client in clients) Console.WriteLine($"  - {client.Name,-32} ( {client.ID} )");
			Console.WriteLine();


			var projects = new List<ClockifyProject>();
			foreach (var ws in workspaces) projects.AddRange(await api.QueryWorkspaceProjects(ws, clients));
			Console.WriteLine($"Found {projects.Count} Projects:");
			foreach (var project in projects) Console.WriteLine($"  - {project.Name,-55} ( {project.ID} )");
			Console.WriteLine();


			var tasks = new List<ClockifyTask>();
			foreach (var proj in projects) tasks.AddRange(await api.QueryProjectTasks(proj));
			Console.WriteLine($"Found {tasks.Count} Tasks:");
			foreach (var task in tasks) Console.WriteLine($"  - {task.Name,-40}  @ {task.Project.Name,-40} ( {task.ID} )");
			Console.WriteLine();

			var entries = new List<ClockifyTimeEntry>();
			foreach (var usr in users)
			{
				foreach (var ws in usr.Workspaces)
				{
					var wue = await api.QueryUserTimeEntries(usr, ws, projects, tasks);
					if (wue.Count == 0) continue;

					Console.WriteLine($"Found {wue.Count,-4} Entries for {usr.Name,-16} in {ws.Name,-16}");

					entries.AddRange(wue);
				}
			}
            Console.WriteLine();


			Console.WriteLine($"Output to '{arg_filepath}'");
			sql.Write(workspaces, users, clients, projects, tasks, entries);
            Console.WriteLine();
		}
	}
}
