namespace Common.Serialization
{
	using Common.Serialization.Serializers;

	public static class Serializer
	{
		public static ISerializer Json => new JsonSerializer();

		public static ISerializer Xml => new XmlSerializer();

		public static ISerializer Binary => new BinarySerializer();
	}
}