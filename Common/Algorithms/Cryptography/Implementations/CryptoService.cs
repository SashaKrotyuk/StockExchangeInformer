namespace Common.Algorithms.Cryptography.Implementations
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Text;
	using System.Xml.Linq;

	using Common.Algorithms.Cryptography.Contracts;
	using Common.Algorithms.Cryptography.Helpers;

	public class CryptoService : ICryptoService
	{
		private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		private static readonly char[] AllowedCharSet = new HashSet<char>(AllowedChars).ToArray();
		private static readonly char[] Padding = { '=' };

		static CryptoService()
		{
			Instance = new CryptoService();
		}

		private CryptoService()
		{
		}

		public static CryptoService Instance { get; private set; }

		public string RandomString(int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(length), "length cannot be less than zero.");
			}

			const int ByteSize = 0x100;
			var allowedCharSetLen = AllowedCharSet.Length;

			if (ByteSize < allowedCharSetLen)
			{
				throw new ArgumentException($"allowed chars may contain no more than {ByteSize} characters.");
			}

			using (var rng = new RNGCryptoServiceProvider())
			{
				var result = new StringBuilder();
				var buf = new byte[128];

				while (result.Length < length)
				{
					rng.GetBytes(buf);

					for (var i = 0; i < buf.Length && result.Length < length; ++i)
					{
						// Divide the byte into allowedCharSet-sized groups. If the
						// random value falls into the last group and the last group is
						// too small to choose from the entire AllowedCharSet, ignore
						// the value in order to avoid biasing the result.
						var outOfRangeStart = ByteSize - (ByteSize % allowedCharSetLen);

						if (outOfRangeStart <= buf[i])
						{
							continue;
						}

						result.Append(AllowedCharSet[buf[i] % allowedCharSetLen]);
					}
				}

				return result.ToString();
			}
		}

		public string GenerateHash(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentException("data must not be empty");
			}

			using (var hashAlgorithm = new SHA384Managed())
			{
				return Convert.ToBase64String(hashAlgorithm.ComputeHash(data));
			}
		}

		public string GenerateSaltedHash(string text, string salt)
		{
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentException("text must not be empty");
			}

			if (string.IsNullOrEmpty(salt))
			{
				throw new ArgumentException("salt must not be empty");
			}

			using (var hashAlgorithm = new SHA384Managed())
			{
				return Convert.ToBase64String(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(text + salt)));
			}
		}

		public byte[] EncryptAES(string inputString)
		{
			return SimpleAes.Encrypt(inputString);
		}

		public string EncryptAESToBase64String(string inputString)
		{
			return Convert.ToBase64String(SimpleAes.Encrypt(inputString)).TrimEnd(Padding).Replace('+', '-').Replace('/', '_');
		}

		public string DecryptAES(byte[] cipherBytes)
		{
			return SimpleAes.Decrypt(cipherBytes);
		}

		public string DecryptAESFromBase64String(string cipher)
		{
			string htmlUnsafeCipher = cipher.Replace('_', '/').Replace('-', '+');
			switch (cipher.Length % 4)
			{
				case 2:
					{
						htmlUnsafeCipher += "==";
						break;
					}

				case 3:
					{
						htmlUnsafeCipher += "=";
						break;
					}
			}

			var cipherBytes = Convert.FromBase64String(htmlUnsafeCipher);

			return SimpleAes.Decrypt(cipherBytes);
		}

		public string EncryptStringRSA(string inputString, string containerName)
		{
			// Create the CspParameters object and set the key container 
			// name used to store the RSA key pair.
			var cp = new CspParameters
			{
				KeyContainerName = containerName,
				Flags = CspProviderFlags.UseMachineKeyStore | CspProviderFlags.UseExistingKey
			};
			var rsaCryptoServiceProvider = new RSACryptoServiceProvider(cp);

			return this.EncryptStringRSA(inputString, rsaCryptoServiceProvider);
		}

		public string EncryptStringRSA(string inputString, XElement xml)
		{
			var rsaCryptoServiceProvider = new RSACryptoServiceProvider();
			rsaCryptoServiceProvider.FromXmlString(xml.ToString());

			return this.EncryptStringRSA(inputString, rsaCryptoServiceProvider);
		}

		public string DecryptStringRSA(string inputString, string containerName)
		{
			// Create the CspParameters object and set the key container 
			// name used to store the RSA key pair.
			var cp = new CspParameters
			{
				KeyContainerName = containerName,
				Flags = CspProviderFlags.UseMachineKeyStore | CspProviderFlags.UseExistingKey
			};
			var rsaCryptoServiceProvider = new RSACryptoServiceProvider(cp);

			return this.DecryptStringRSA(inputString, rsaCryptoServiceProvider);
		}

		public string DecryptStringRSA(string inputString, XElement xml)
		{
			var rsaCryptoServiceProvider = new RSACryptoServiceProvider();
			rsaCryptoServiceProvider.FromXmlString(xml.ToString());

			return this.DecryptStringRSA(inputString, rsaCryptoServiceProvider);
		}

		public string GetOrCreateKeyInContainerRSA(string containerName)
		{
			// Create the CspParameters object and set the key container 
			// name used to store the RSA key pair.
			var cp = new CspParameters
			{
				KeyContainerName = containerName,
				Flags = CspProviderFlags.UseMachineKeyStore | CspProviderFlags.UseArchivableKey
			};

			// Create a new instance of RSACryptoServiceProvider that accesses
			// the key container MyKeyContainerName.
			var rsa = new RSACryptoServiceProvider(cp);

			return rsa.ToXmlString(true);
		}

		public void DeleteKeyFromContainerRSA(string containerName)
		{
			// Create the CspParameters object and set the key container 
			// name used to store the RSA key pair.
			var cp = new CspParameters
			{
				KeyContainerName = containerName,
				Flags = CspProviderFlags.UseMachineKeyStore
			};

			// Create a new instance of RSACryptoServiceProvider that accesses
			// the key container.
			var rsa = new RSACryptoServiceProvider(cp) { PersistKeyInCsp = false };

			// Delete the key entry in the container.

			// Call Clear to release resources and delete the key from the container.
			rsa.Clear();
		}

		private string EncryptStringRSA(string inputString, RSACryptoServiceProvider rsaCryptoServiceProvider)
		{
			var keySize = rsaCryptoServiceProvider.KeySize;
			keySize = keySize / 8;
			var bytes = Encoding.Unicode.GetBytes(inputString);
			var maxLength = keySize - 42;
			var dataLength = bytes.Length;
			var iterations = dataLength / maxLength;
			var stringBuilder = new StringBuilder();

			for (var i = 0; i <= iterations; i++)
			{
				var iterMaxLen = maxLength * i;
				var tempBytesLen = (dataLength - iterMaxLen > maxLength) ? maxLength : dataLength - iterMaxLen;
				var tempBytes = new byte[tempBytesLen];
				Buffer.BlockCopy(bytes, iterMaxLen, tempBytes, 0, tempBytes.Length);
				var encryptedBytes = rsaCryptoServiceProvider.Encrypt(tempBytes, true);
				stringBuilder.Append(Convert.ToBase64String(encryptedBytes));
			}

			return stringBuilder.ToString();
		}

		private string DecryptStringRSA(string inputString, RSACryptoServiceProvider rsaCryptoServiceProvider)
		{
			var keySize = rsaCryptoServiceProvider.KeySize;
			var base64BlockSize = ((keySize / 8) % 3 != 0) ? (((keySize / 8) / 3) * 4) + 4 : ((keySize / 8) / 3) * 4;
			var iterations = inputString.Length / base64BlockSize;
			var arrayList = new ArrayList();

			for (var i = 0; i < iterations; i++)
			{
				var encryptedBytes = Convert.FromBase64String(inputString.Substring(base64BlockSize * i, base64BlockSize));
				arrayList.AddRange(rsaCryptoServiceProvider.Decrypt(encryptedBytes, true));
			}

			return Encoding.Unicode.GetString((byte[])arrayList.ToArray(typeof(byte)));
		}
	}
}