namespace ClockifyAPIClient.Model
{
	class ClockifyClient
	{
		public readonly string ID;
		public readonly string Name;
        public readonly ClockifyWorkspace Workspace;
        public readonly bool Archived;

        public ClockifyClient(string id, string name, bool a, ClockifyWorkspace ws)
		{
			ID = id;
			Name = name;
            Workspace = ws;
            Archived = a;
		}
	}
}
