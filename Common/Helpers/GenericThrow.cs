namespace Common.Helpers
{
	using System;

	public static class Throw<TException>
		where TException : Exception
	{
		/// <summary>
		/// Throws an exception of type <see cref="TException"/> if the condition is true
		/// </summary>
		public static void If(bool condition, string message)
		{
			if (condition)
			{
				throw Create(message);
			}
		}

		/// <summary>
		/// As <see cref="Throw.If(bool, string)"/>, but allows the message to be specified lazily. The message function will only be evaluated
		/// if the condition is true
		/// </summary>
		public static void If(bool condition, Func<string> message)
		{
			if (condition)
			{
				throw Create(message());
			}
		}

		private static TException Create(string message)
		{
			return (TException)Activator.CreateInstance(typeof(TException), message);
		}
	}
}