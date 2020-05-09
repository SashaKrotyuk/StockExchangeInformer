namespace Common.Extensions
{
	using System;
	using System.IO;
	using System.Text;
	using System.Xml.Linq;
	using System.Xml.Serialization;
	using Common.Helpers;

	public static class ObjectExtensions
	{
		public static bool Value(this bool? item)
		{
			return item.HasValue && item.Value;
		}

		/// <summary>
		/// Bounds a value within a range
		/// </summary>
		public static T Capped<T>(this T @this, T? min = null, T? max = null)
			where T : struct, IComparable<T>
		{
			return min.HasValue && @this.CompareTo(min.Value) < 0 ? min.Value
				: max.HasValue && @this.CompareTo(max.Value) > 0 ? max.Value
				: @this;
		}

		/// <summary>
		/// Type-safely casts the given value to the specified type
		/// </summary>
		public static T As<T>(this T @this)
		{
			return @this;
		}

		/// <summary>
		/// Invokes the given function on the given object if and only if the given object is not null. Otherwise,
		/// the value specified by "ifNullReturn" is returned
		/// </summary>
		public static TResult NullSafe<TObj, TResult>(
			this TObj obj,
			Func<TObj, TResult> func,
			TResult ifNullReturn = default(TResult))
		{
			Throw.IfNull(func, "func");
			return obj != null ? func(obj) : ifNullReturn;
		}

		public static XElement ToXml<T>(this T obj)
		{
			using (var memoryStream = new MemoryStream())
			{
				using (TextWriter streamWriter = new StreamWriter(memoryStream))
				{
					var xmlSerializer = new XmlSerializer(typeof(T));
					xmlSerializer.Serialize(streamWriter, obj);

					return XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()));
				}
			}
		}

		public static T FromXml<T>(this XElement obj)
		{
			var serializer = new XmlSerializer(typeof(T));

			return (T)serializer.Deserialize(obj.CreateReader());
		}

		public static T FromXml<T>(this string obj)
		{
			return FromXml<T>(XElement.Parse(obj));
		}
	}
}