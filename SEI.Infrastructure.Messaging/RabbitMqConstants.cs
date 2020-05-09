namespace SEI.Infrastructure.Messaging
{
	public static class RabbitMqConstants
	{
#if DEBUG
		public const string RabbitMqUri = "rabbitmq://localhost/sei/";
		public const string UserName = "guest";
		public const string Password = "guest";
#endif

#if !DEBUG
		public const string RabbitMqUri = "rabbitmq://localhost/sei/";
		public const string UserName = "guest";
		public const string Password = "guest";
#endif
	}
}