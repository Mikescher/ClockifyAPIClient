namespace ClockifyAPIClient.Model
{
	class ClockifyWorkspace
	{
		public readonly string ID;
		public readonly string Name;

		public ClockifyWorkspace(string id, string name)
		{
			ID = id;
			Name = name;
		}
	}
}
