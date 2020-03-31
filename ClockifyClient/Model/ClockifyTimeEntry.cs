using System;

namespace ClockifyAPIClient.Model
{
	class ClockifyTimeEntry
	{
		public readonly string ID;
		public readonly string Description;
        public readonly ClockifyUser User;
        public readonly ClockifyWorkspace Workspace;
        public readonly ClockifyProject Project;
        public readonly ClockifyTask Task;
        public readonly bool Billable;

        public readonly DateTime Start;
        public readonly DateTime End;
        public readonly TimeSpan Duration;

        public ClockifyTimeEntry(string id, string desc, bool bill, ClockifyUser u, ClockifyWorkspace ws, ClockifyProject proj, ClockifyTask task, DateTime start, DateTime end, TimeSpan ts)
        {
            ID          = id;
            Description = desc;
            Workspace   = ws;
            Project     = proj;
            Task        = task;
            Start       = start;
            End         = end;
            Duration    = ts;
            Billable    = bill;
            User        = u;
        }
    }
}
