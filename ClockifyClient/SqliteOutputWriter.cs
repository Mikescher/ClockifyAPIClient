using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using ClockifyAPIClient.Model;
using ClockifyAPIClient.Properties;

namespace ClockifyAPIClient
{
	class SqliteOutputWriter
	{
		private readonly string _filepath;

		public SqliteOutputWriter(string filepath)
		{
			_filepath = filepath;

			if (File.Exists(filepath)) File.Delete(filepath);
		}

		public void Write(List<ClockifyWorkspace> workspaces, List<ClockifyUser> users, List<ClockifyClient> clients, List<ClockifyProject> projects, List<ClockifyTask> tasks, List<ClockifyTimeEntry> entries)
		{
			var sb = new SQLiteConnectionStringBuilder
			{
				DataSource     = _filepath,
				DefaultTimeout = 5000,
				FailIfMissing  = false,
				ReadOnly       = false,
				JournalMode    = SQLiteJournalModeEnum.Memory,
				SyncMode       = SynchronizationModes.Off,
			};

			using var conn = new SQLiteConnection(sb.ToString());
			conn.Open();

			var cmd0 = conn.CreateCommand();
            cmd0.CommandText = Resources.schema;
            cmd0.ExecuteNonQuery();

            cmd0.CommandText = Resources.views;
            cmd0.ExecuteNonQuery();

            var t = conn.BeginTransaction();
            {
				var cmd1 = conn.CreateCommand();
				cmd1.CommandText = @"INSERT INTO [workspaces] ([workspace_id], [name]) VALUES (@id, @name)";
				foreach (var val in workspaces)
				{
					cmd1.Parameters.AddWithValue("@id",   val.ID);
					cmd1.Parameters.AddWithValue("@name", val.Name);
					cmd1.ExecuteNonQuery();
				}

				var cmd2 = conn.CreateCommand();
				cmd2.CommandText = @"INSERT INTO [users] ([user_id], [email], [name]) VALUES (@id, @mail, @name)";
				foreach (var val in users)
				{
					cmd2.Parameters.AddWithValue("@id",   val.ID);
					cmd2.Parameters.AddWithValue("@mail", val.Email);
					cmd2.Parameters.AddWithValue("@name", val.Name);
					cmd2.ExecuteNonQuery();
				}

				var cmd3 = conn.CreateCommand();
				cmd3.CommandText = @"INSERT INTO [map_users_workspaces] ([user_id], [workspace_id]) VALUES (@uid, @wid)";
				foreach (var val1 in users)
				{
					foreach (var val2 in val1.Workspaces)
					{
						cmd3.Parameters.AddWithValue("@uid", val1.ID);
						cmd3.Parameters.AddWithValue("@wid", val2.ID);
						cmd3.ExecuteNonQuery();
					}
				}

				var cmd4 = conn.CreateCommand();
				cmd4.CommandText = @"INSERT INTO [clients] ([client_id], [name], [archived], [workspace_id]) VALUES (@id, @name, @arch, @wsid)";
				foreach (var val in clients)
				{
					cmd4.Parameters.AddWithValue("@id", val.ID);
					cmd4.Parameters.AddWithValue("@name", val.Name);
					cmd4.Parameters.AddWithValue("@arch", val.Archived);
					cmd4.Parameters.AddWithValue("@wsid", val.Workspace.ID);
					cmd4.ExecuteNonQuery();
				}

				var cmd5 = conn.CreateCommand();
				cmd5.CommandText = @"INSERT INTO [projects] ([project_id], [name], [archived], [workspace_id], [client_id]) VALUES (@id, @name, @arch, @wsid, @cid)";
				foreach (var val in projects)
				{
					cmd5.Parameters.AddWithValue("@id",   val.ID);
					cmd5.Parameters.AddWithValue("@name", val.Name);
					cmd5.Parameters.AddWithValue("@arch", val.Archived);
					cmd5.Parameters.AddWithValue("@wsid", val.Workspace.ID);
					cmd5.Parameters.AddWithValue("@cid",  val.Client?.ID);
					cmd5.ExecuteNonQuery();
				}

				var cmd6 = conn.CreateCommand();
				cmd6.CommandText = @"INSERT INTO [tasks] ([task_id], [name], [workspace_id], [project_id]) VALUES (@id, @name, @wsid, @pid)";
				foreach (var val in tasks)
				{
					cmd6.Parameters.AddWithValue("@id", val.ID);
					cmd6.Parameters.AddWithValue("@name", val.Name);
					cmd6.Parameters.AddWithValue("@wsid", val.Workspace.ID);
					cmd6.Parameters.AddWithValue("@pid", val.Project?.ID);
					cmd6.ExecuteNonQuery();
				}

				var cmd7 = conn.CreateCommand();
				cmd7.CommandText = @"INSERT INTO [entries] ([entry_id], [description], [workspace_id], [project_id], [task_id], [user_id], [interval_start], [interval_end], [interval_duration], [billable]) VALUES (@id, @desc, @wsid, @pid, @tid, @uid, @ivs, @ive, @ivd, @bill)";
				foreach (var val in entries)
				{
					cmd7.Parameters.AddWithValue("@id",   val.ID);
					cmd7.Parameters.AddWithValue("@desc", val.Description);
					cmd7.Parameters.AddWithValue("@wsid", val.Workspace.ID);
					cmd7.Parameters.AddWithValue("@pid",  val.Project?.ID);
					cmd7.Parameters.AddWithValue("@tid",  val.Task?.ID);
					cmd7.Parameters.AddWithValue("@uid",  val.User.ID);
					cmd7.Parameters.AddWithValue("@bill", val.Billable);
                    cmd7.Parameters.AddWithValue("@ivs",  val.Start.ToLocalTime().ToString("yyyy'-'MM'-'dd HH':'mm':'ss"));
					cmd7.Parameters.AddWithValue("@ive",  val.End.ToLocalTime().ToString("yyyy'-'MM'-'dd HH':'mm':'ss"));
					cmd7.Parameters.AddWithValue("@ivd",  (long)val.Duration.TotalSeconds);
                    cmd7.ExecuteNonQuery();
				}
			}
			t.Commit();
		}
	}
}
