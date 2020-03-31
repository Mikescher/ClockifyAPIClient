using System;
using System.Threading.Tasks;

namespace ClockifyClient
{
	class Program
	{
		static async Task Main(string[] args)
		{
			try
			{
				await Run();
				Console.ReadLine();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		private static async Task Run()
		{
			var api = new ClockifyAPIConnection("W7YNx7B5h0KQx584");

			var user = await api.QueryCurrentUser();

			Console.WriteLine($"Current user: {user.Name} ({user.Email}) ({user.ID})");
		}
	}
}
