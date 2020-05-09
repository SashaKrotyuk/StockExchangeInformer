namespace Common.Algorithms.Cryptography.Helpers
{
	using System.IO;
	using System.Security.Cryptography;

	public static class SimpleAes
	{
		private static readonly byte[] Key = { 201, 217, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37, 112, 222, 19, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 102, 236, 53, 209 };
		private static readonly byte[] IV = { 15, 64, 191, 111, 23, 3, 113, 119, 231, 121, 3, 112, 79, 32, 114, 156 };

		private static readonly RijndaelManaged RM;
		private static readonly System.Text.UTF8Encoding Encoder;

		static SimpleAes()
		{
			RM = new RijndaelManaged();
			Encoder = new System.Text.UTF8Encoding();
		}

		/// Encrypt some text and return an encrypted byte array.
		public static byte[] Encrypt(string clearText)
		{
			var inputBytes = Encoder.GetBytes(clearText);
			using (var input = new MemoryStream(inputBytes))
			{
				using (var output = new MemoryStream())
				{
					var encryptor = RM.CreateEncryptor(Key, IV);
					using (var cryptStream = new CryptoStream(output, encryptor, CryptoStreamMode.Write))
					{
						var buffer = new byte[1024];
						var read = input.Read(buffer, 0, buffer.Length);
						while (read > 0)
						{
							cryptStream.Write(buffer, 0, read);
							read = input.Read(buffer, 0, buffer.Length);
						}

						cryptStream.FlushFinalBlock();
						return output.ToArray();
					}
				}
			}
		}

		public static string Decrypt(byte[] cipherBytes)
		{
			using (var input = new MemoryStream(cipherBytes))
			{
				using (var output = new MemoryStream())
				{
					var decryptor = RM.CreateDecryptor(Key, IV);
					using (var cryptStream = new CryptoStream(input, decryptor, CryptoStreamMode.Read))
					{
						var buffer = new byte[1024];
						var read = cryptStream.Read(buffer, 0, buffer.Length);
						while (read > 0)
						{
							output.Write(buffer, 0, read);
							read = cryptStream.Read(buffer, 0, buffer.Length);
						}

						cryptStream.Flush();
						return Encoder.GetString(output.ToArray());
					}
				}
			}
		}
	}
}