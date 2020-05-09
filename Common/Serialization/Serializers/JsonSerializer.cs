namespace Common.Serialization.Serializers
{
	using System;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	/// <summary>
	/// Serializes and deserializes objects using JSON
	/// </summary>
	/// <remarks>
	/// Internally uses JSON.NET for friendly rendering of dates and enums
	/// </remarks>
	public class JsonSerializer : ISerializer
	{
		public SerializationFormat Format => SerializationFormat.Json;

		/// <summary>
		/// Serialize to JSON format
		/// </summary>
		/// <param name="type"></param>
		/// <param name="returnValue"></param>
		/// <returns></returns>
		public object Serialize(object value)
		{
			return JsonConvert.SerializeObject(value, GetSettings());
		}

		/// <summary>
		/// Deserialize from JSON format
		/// </summary>
		/// <param name="type"></param>
		/// <param name="cachedValue"></param>
		/// <returns></returns>
		public object Deserialize(Type type, object serializedValue)
		{
			return JsonConvert.DeserializeObject((string)serializedValue, type, GetSettings());
		}

		private static JsonSerializerSettings GetSettings()
		{
			var settings = new JsonSerializerSettings
			{
				Converters = new JsonConverter[]
				{
					new StringEnumConverter()
				}
			};
			return settings;
		}
	}
}