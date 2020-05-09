namespace Common.Serialization.Serializers
{
	using System;

	/// <summary>
	/// Null implementaion of <see cref="ISerializer"/>
	/// </summary>
	public class NullSerializer : ISerializer
	{
		public SerializationFormat Format => SerializationFormat.None;

		/// <summary>
		/// Returns the given object
		/// </summary>
		/// <param name="type"></param>
		/// <param name="serializedValue"></param>
		/// <returns></returns>
		public object Deserialize(Type type, object serializedValue)
		{
			return serializedValue;
		}

		/// <summary>
		/// Returns the given object
		/// </summary>
		/// <param name="returnValue"></param>
		/// <returns></returns>
		public object Serialize(object returnValue)
		{
			return returnValue;
		}
	}
}