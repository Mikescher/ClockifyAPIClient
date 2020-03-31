using System;

namespace ClockifyAPIClient
{
	class APIException : Exception
	{
		public APIException(string msg): base(msg)
		{
			
		}
	}
}
