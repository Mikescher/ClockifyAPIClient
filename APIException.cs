using System;

namespace ClockifyClient
{
	class APIException : Exception
	{
		public APIException(string msg): base(msg)
		{
			
		}
	}
}
