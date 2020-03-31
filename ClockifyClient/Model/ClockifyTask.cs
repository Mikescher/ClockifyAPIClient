namespace ClockifyAPIClient.Model
{
	class ClockifyTask
	{
		public readonly string ID;
		public readonly string Name;
		public readonly ClockifyWorkspace Workspace;
		public readonly ClockifyProject Project;

        public ClockifyTask(string id, string name, ClockifyProject proj, ClockifyWorkspace ws)
		{
			ID = id;
			Name = name;
            Workspace = ws;
			Project = proj;
		}
	}
}
