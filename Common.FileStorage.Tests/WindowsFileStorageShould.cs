namespace Common.FileStorage.Tests
{
	using System.IO;
	using System.Text;
	using System.Threading.Tasks;

	using Algorithms.Cryptography.Implementations;
	using Common.FileStorage.Contracts;
	using Common.FileStorage.Implementation;
	using NUnit.Framework;
	using Shouldly;
	
	[TestFixture]
	public class WindowsFileStorageShould
	{
		private const string InputFile = "Input\\Input.docx";
		private const string ExistingFile = "Output\\Output.pdf";
		private const string OutputDirectory = "Output\\";
		private IFileStorage fileStorage;

		[SetUp]
		public async Task SetUp()
		{
			var inputDirectory = Path.GetDirectoryName(InputFile);

			if (!Directory.Exists(inputDirectory))
			{
				Directory.CreateDirectory(inputDirectory);
			}

			if (!Directory.Exists(OutputDirectory))
			{
				Directory.CreateDirectory(OutputDirectory);
			}

			await this.GenerateFileWithRandomContent(InputFile, 20);
			await this.GenerateFileWithRandomContent(ExistingFile, 20);

			this.fileStorage = new WindowsSystemFileStorage(OutputDirectory);
		}

		[TearDown]
		public void TearDown()
		{
			DirectoryInfo di = new DirectoryInfo(OutputDirectory);

			foreach (FileInfo file in di.GetFiles())
			{
				if (file.Name != "Output.pdf")
				{
					file.Delete();
				}
			}

			foreach (DirectoryInfo dir in di.GetDirectories())
			{
				dir.Delete(true);
			}

			var inputDirectory = Path.GetDirectoryName(InputFile);

			di = new DirectoryInfo(inputDirectory);

			foreach (FileInfo file in di.GetFiles())
			{
				file.Delete();
			}

			foreach (DirectoryInfo dir in di.GetDirectories())
			{
				dir.Delete(true);
			}
		}

		[Test]
		public async Task CreateFileOnDisk()
		{
			// Arrange
			var file = File.ReadAllBytes(InputFile);

			// Act
			var uploadedFile = await this.fileStorage.CreateFileAsync(file);

			// Assert
			var resultingFile = File.ReadAllBytes(uploadedFile.DownloadLink);

			resultingFile.Length.ShouldBe(file.Length);
		}

		[Test]
		public async Task CreateFoldersIfNeeded()
		{
			// Arrange
			var file = File.ReadAllBytes(InputFile);

			// Act
			var uploadedFile = await this.fileStorage.CreateFileAsync("Documents/testFile.docx", file);

			// Assert
			var resultingFile = File.ReadAllBytes(uploadedFile.DownloadLink);

			resultingFile.Length.ShouldBe(file.Length);
		}

		[Test]
		public async Task ReadFileFromDisk()
		{
			var file = await this.fileStorage.ReadFileAsync(ExistingFile);

			file.FileName.ShouldBe("Output.pdf");
			file.Content.Length.ShouldBeGreaterThan(0);
		}

		[Test]
		public async Task UpdateFileOnDisk()
		{
			var existingFile = await this.fileStorage.ReadFileAsync(ExistingFile);

			var fileContent = this.GenerateRandomContent(50);
			var updatedFile = await this.fileStorage.UpdateFileAsync(ExistingFile, fileContent);

			existingFile.DownloadLink.ShouldBe(updatedFile.DownloadLink);
			existingFile.Content.Length.ShouldBeLessThan(updatedFile.Content.Length);
		}

		[Test]
		public async Task UpdateFileOnDiskOnSpecificFolder()
		{
			var fileAddress = "Documents/testFile.docx";
			var fileContent = this.GenerateRandomContent(50);
			var createdFile = await this.fileStorage.CreateFileAsync(fileAddress, fileContent);
			
			var newFileContent = this.GenerateRandomContent(100);
			var updatedFile = await this.fileStorage.UpdateFileAsync(createdFile.DownloadLink, newFileContent);

			createdFile.DownloadLink.ShouldBe(updatedFile.DownloadLink);
			createdFile.Content.Length.ShouldBeLessThan(updatedFile.Content.Length);
		}

		[Test]
		public async Task DeleteFileFromDisk()
		{
			var file = await this.fileStorage.DeleteFileAsync(ExistingFile);

			file.FileName.ShouldBe("Output.pdf");
			file.Content.Length.ShouldBeGreaterThan(0);

			File.Exists(file.DownloadLink).ShouldBeFalse();
		}

		[Test]
		public void GenerateFileName()
		{
			var fileName = this.fileStorage.GenerateFileName();

			fileName.ShouldNotBeNullOrEmpty();
			fileName.ShouldEndWith(".file");
		}

		private async Task GenerateFileWithRandomContent(string filePath, int contentLength)
		{
			var content = this.GenerateRandomContent(contentLength);

			using (FileStream destinationStream = File.Create(filePath))
			{
				await destinationStream.WriteAsync(content, 0, content.Length);
			}
		}

		private byte[] GenerateRandomContent(int contentLength)
		{
			var randomString = CryptoService.Instance.RandomString(contentLength);
			var stringBytes = Encoding.ASCII.GetBytes(randomString);

			return stringBytes;
		}
	}
}
