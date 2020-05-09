namespace Common.Exceptions
{
	using System;

	public class HandledException : Exception
	{
		public HandledException(string message) : base(message)
		{
		}
	}
}