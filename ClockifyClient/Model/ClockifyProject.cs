namespace ClockifyAPIClient.Model
{
	class ClockifyProject
	{
		public readonly string ID;
		public readonly string Name;
		public readonly bool Archived;
		public readonly ClockifyWorkspace Workspace;
        public readonly ClockifyClient Client;

        public ClockifyProject(string id, string name, bool a, ClockifyWorkspace ws, ClockifyClient client)
		{
			ID = id;
			Name = name;
            Workspace = ws;
            Client = client;
            Archived = a;
		}
	}
}
