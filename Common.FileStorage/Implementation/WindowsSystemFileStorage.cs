namespace Common.FileStorage.Implementation
{
	using System;
	using System.IO;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;

	using Common.Algorithms.Cryptography.Implementations;
	using Common.FileStorage.Contracts;
	
	public class WindowsSystemFileStorage : IFileStorage
	{
		private readonly string outputDirectory;

		public WindowsSystemFileStorage(string outputDirectory)
		{
			outputDirectory = outputDirectory.TrimEnd('/', '\\');
			outputDirectory = this.AdjustPath(outputDirectory);

			this.outputDirectory = outputDirectory;
		}

		public async Task<StorageFileInfo> ReadFileAsync(string fileAddress)
		{
			using (FileStream fileBytes = new FileStream(fileAddress, FileMode.Open))
			{
				var result = new StorageFileInfo
				{
					FileName = Path.GetFileName(fileAddress),
					DownloadLink = fileAddress
				};

				result.Content = new byte[fileBytes.Length];
				await fileBytes.ReadAsync(result.Content, 0, (int)fileBytes.Length);

				return result;
			}
		}

		public string GenerateFileName()
		{
			return $"{Guid.NewGuid().ToString()}.file";
		}

		public async Task<StorageFileInfo> CreateFileAsync(byte[] file)
		{
			var fileName = this.GenerateFileName();

			return await this.CreateFileAsync(fileName, file);
		}

		public async Task<StorageFileInfo> CreateFileAsync(string fileName, byte[] file)
		{
			fileName = this.AdjustPath(fileName);

			using (MemoryStream inputStream = new MemoryStream(file))
			{
				string filePath = fileName;

				// Replace file name with random string
				string pattern = @"(.*(\/|\\))([\d\w\W]*)\.(.*)";
				string uniqueName = CryptoService.Instance.RandomString(10);
				string uniqueFileName = Regex.Replace(@fileName, pattern, m => $"{m.Groups[1].Value}{uniqueName}.{m.Groups[4].Value}");
				if (!fileName.Contains(this.outputDirectory))
				{
					filePath = $@"{outputDirectory}\{uniqueFileName}";
				}

				var directory = Path.GetDirectoryName(filePath);

				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}

				using (FileStream destinationStream = File.Create(filePath))
				{
					await inputStream.CopyToAsync(destinationStream);

					var result = new StorageFileInfo
					{
						FileName = fileName,
						Location = filePath,
						DownloadLink = filePath,
						Content = file
					};

					return result;
				}
			}
		}

		public async Task<StorageFileInfo> UpdateFileAsync(string existingFile, byte[] newFile)
		{
			var deletedFile = await this.DeleteFileAsync(existingFile);

			return await this.CreateFileAsync(existingFile, newFile);
		}

		public async Task<StorageFileInfo> DeleteFileAsync(string existingFile)
		{
			var existingFileInfo = await this.ReadFileAsync(existingFile);

			var fileName = Path.GetFileName(existingFile);
			var filePath = Path.GetDirectoryName(existingFile);

			var di = new DirectoryInfo(filePath);

			foreach (FileInfo file in di.GetFiles(fileName))
			{
				file.Delete();
			}

			return existingFileInfo;
		}

		private string AdjustPath(string path)
		{
			var result = path.Replace('/', '\\');
			return result;
		}
	}
}