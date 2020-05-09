namespace Common.Tests.Helpers
{
	using Common.Helpers;
	using NUnit.Framework;
	using System.Configuration;

	[TestFixture]
	public class FileLinkHelperShould
	{
		[Test]
		public void GenerateFileDownloadLink()
		{
			var apiAddress = ConfigurationManager.AppSettings["DownloadFileAddress"];
			var fileLocation = @"C:\AMPFiles\AmpToday\Public\Documents\Annual_safety_inspection_guide (3).pdf";
			var encryptedFileLocation = "1ItWYjp_1onMH_JmIQR05HyNGy7Ro-wR8RSqn9n4WJIhd2ChgfY3l54_Ig5fxjGWC0ZW6yYE5bPeQoJiBWHwRluPENYZYN6Xgno6Ajenc4I";
			var fileDownloadLink = $"{apiAddress}/{encryptedFileLocation}";

			var result = FileLinkHelper.GenerateFileDownloadLink(fileLocation);
			Assert.AreEqual(fileDownloadLink, result);
		}

		[Test]
		public void DecryptFileLink()
		{
			var link = "1ItWYjp_1onMH_JmIQR05HyNGy7Ro-wR8RSqn9n4WJIhd2ChgfY3l54_Ig5fxjGWC0ZW6yYE5bPeQoJiBWHwRluPENYZYN6Xgno6Ajenc4I";
			var decryptedLink = @"C:\AMPFiles\AmpToday\Public\Documents\Annual_safety_inspection_guide (3).pdf";

			var result = FileLinkHelper.DecryptFileLink(link);
			Assert.AreEqual(decryptedLink, result);
		}

		[Test]
		public void EncryptFileLink()
		{
			var link = @"C:\AMPFiles\AmpToday\Public\Documents\Annual_safety_inspection_guide (3).pdf";
			var encryptedLink = "1ItWYjp_1onMH_JmIQR05HyNGy7Ro-wR8RSqn9n4WJIhd2ChgfY3l54_Ig5fxjGWC0ZW6yYE5bPeQoJiBWHwRluPENYZYN6Xgno6Ajenc4I";

			var result = FileLinkHelper.EncryptFileLink(link);
			Assert.AreEqual(encryptedLink, result);
		}
	}
}
