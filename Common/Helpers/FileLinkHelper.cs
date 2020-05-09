namespace Common.Helpers
{
	using System.Configuration;

	using Common.Algorithms.Cryptography.Implementations;

	public class FileLinkHelper
	{
		private static string apiAddress;

		static FileLinkHelper()
		{
			apiAddress = ConfigurationManager.AppSettings["DownloadFileAddress"];
		}

		public static string GenerateFileDownloadLink(string fileLocation)
		{
			var encryptedFileLocation = CryptoService.Instance.EncryptAESToBase64String(fileLocation);

			return $"{apiAddress}/{encryptedFileLocation}";
		}

		public static string DecryptFileLink(string link)
		{
			return CryptoService.Instance.DecryptAESFromBase64String(link);
		}

		public static string EncryptFileLink(string link)
		{
			return CryptoService.Instance.EncryptAESToBase64String(link);
		}
	}
}
