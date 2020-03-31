using System;
using System.Collections.Generic;
using System.Text;

namespace ClockifyClient.Model
{
	class ClockifyUser
	{
		public readonly string ID;
		public readonly string Email;
		public readonly string Name;

		public ClockifyUser(string iD, string email, string name)
		{
			ID = iD;
			Email = email;
			Name = name;
		}
	}
}
