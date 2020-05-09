namespace Common.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Text;
	using System.Text.RegularExpressions;

	/// <summary>
	/// Extension methods for the <see cref="string"/> class.
	/// </summary>
	public static class StringExtensions
    {
		private const char Quote = '"';

		/// <summary>
		/// Returns the result of calling <seealso cref="string.Format(string,object[])"/> with the supplied arguments.
		/// </summary>
		/// <remarks>
		/// Uses <see cref="CultureInfo.InvariantCulture"/> to format
		/// </remarks>
		/// <param name="formatString">The format string</param>
		/// <param name="args">The values to be formatted</param>
		/// <returns>The formatted string</returns>
		public static string FormatWith(this string formatString, params object[] args)
        {
            return args == null || args.Length == 0 ? formatString : string.Format(formatString, args);
        }

        /// <summary>
        /// Tests the string to see if it is null or "".
        /// </summary>
        /// <returns>True if null or "".</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Tests the string to see if it is null, empty or consists only of whitespace.
        /// </summary>
        /// <returns>True if null or "".</returns>
        public static bool IsNullOrWhitespace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

		public static string IfEmptyThen(this string a, string b)
		{
			return !string.IsNullOrWhiteSpace(a) ? a : b;
		}

		public static string IfEmptyThen(this string a, Func<string> func)
		{
			return !string.IsNullOrWhiteSpace(a) ? a : func();
		}

		public static string StripHtml(this string input)
		{
			var inputWithoutTags = Regex.Replace(input, "<.*?>", string.Empty);
			var inputWithoutTabulationAndNewLines = Regex.Replace(inputWithoutTags, @"\t|\n|\r", string.Empty);

			return inputWithoutTabulationAndNewLines.Trim();
		}

		// Note: .Trim() applied for each column in line
		public static string[] ParseCsvLine(this string line, char csvSeparator)
		{
			var result = new List<string>();
			var sb = new StringBuilder(line.Length);
			bool inQuote = false;

			Action flush = () =>
			{
				result.Add(sb.ToString().Trim());
				sb.Length = 0;
				inQuote = false;
			};

			for (int i = 0; i < line.Length; i++)
			{
				var c = line[i];
				bool isCsvSeparator = c == csvSeparator;

				if (isCsvSeparator)
				{
					if (inQuote)
					{
						sb.Append(c);
					}
					else
					{
						flush();
					}
				}
				else if (c == Quote)
				{
					if (inQuote)
					{
						if (i + 1 < line.Length && line[i + 1] == Quote)
						{
							i++;
							sb.Append(c);
						}
						else
						{
							if (i + 1 == line.Length || line[i + 1] == csvSeparator)
							{
								inQuote = false;
							}
							else
							{
								var message = $"Unexpected quote in position {i} for line [{line}]";
								throw new IndexOutOfRangeException(message);
							}
						}
					}
					else
					{
						if (sb.Length == 0)
						{
							inQuote = true;
						}
						else
						{
							var message = $"Unexpected quote in position {i} for line [{line}]";
							throw new IndexOutOfRangeException(message);
						}
					}
				}
				else
				{
					sb.Append(c);
				}
			}

			flush();

			return result.ToArray();
		}

		public static string CapitalizeFirstLetter(this string s)
		{
			if (string.IsNullOrWhiteSpace(s) || s.Length == 1)
			{
				return s;
			}

			return s.Substring(0, 1).ToUpper() + s.Substring(1);
		}

		public static string Trim(this string s, int length)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}

			if (s.Length < length)
			{
				length = s.Length;
			}

			return s.Substring(0, length);
		}

		public static string ToBase64(this string value)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
		}

		public static string FromBase64(this string value)
		{
			return Encoding.UTF8.GetString(Convert.FromBase64String(value));
		}

		public static bool IsValidEmail(this string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				return false;
			}
			
			try
			{
				email = Regex.Replace(email, @"(@)(.+)$", DomainEvaluator.DomainMapper, RegexOptions.None);
			}
			catch
			{
				return false;
			}

			// Return true if strIn is in valid e-mail format. 
			try
			{
				return Regex.IsMatch(
					email,
					@"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,24}))$",
					RegexOptions.IgnoreCase);
			}
			catch
			{
				return false;
			}
		}

		private static class DomainEvaluator
		{
			public static string DomainMapper(Match match)
			{
				// IdnMapping class with default property values.
				var idn = new IdnMapping();

				var domainName = match.Groups[2].Value;
				domainName = idn.GetAscii(domainName);

				return match.Groups[1].Value + domainName;
			}
		}
	}
}