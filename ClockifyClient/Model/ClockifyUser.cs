using System.Collections.Generic;
using System.Linq;

namespace ClockifyAPIClient.Model
{
	class ClockifyUser
	{
		public readonly string ID;
		public readonly string Email;
		public readonly string Name;
		public readonly List<ClockifyWorkspace> Workspaces;

        public ClockifyUser(string id, string email, string name, ClockifyWorkspace ws)
        {
            ID = id;
            Email = email;
            Name = name;
            Workspaces = ws == null ? new List<ClockifyWorkspace>() : new List<ClockifyWorkspace> { ws };
		}

        private ClockifyUser(string id, string email, string name, List<ClockifyWorkspace> wsl)
        {
            ID = id;
            Email = email;
            Name = name;
            Workspaces = wsl;
        }

		public static ClockifyUser Merge(List<ClockifyUser> val)
        {
            if (val.GroupBy(p => p.ID).Count()    != 1) throw new APIException("ClockifyUser.Merge failed");
            if (val.GroupBy(p => p.Email).Count() != 1) throw new APIException("ClockifyUser.Merge failed");
            if (val.GroupBy(p => p.Name).Count()  != 1) throw new APIException("ClockifyUser.Merge failed");

			return new ClockifyUser(val[0].ID, val[0].Email, val[0].Name, val.SelectMany(p => p.Workspaces).GroupBy(w => w.ID).Select(w => w.First()).ToList());
		}
    }
}
