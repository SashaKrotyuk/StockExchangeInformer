namespace Common.Extensions
{
	using Common.Serialization;

	/// <summary>
	/// Extensions to <see cref="ISerializer"/>
	/// </summary>
	public static class SerializerExtensions
    {
        public static T Deserialize<T>(this ISerializer serializer, object serializedValue) where T : class
        {
            return serializer.Deserialize(typeof(T), serializedValue) as T;
        }
    }
}