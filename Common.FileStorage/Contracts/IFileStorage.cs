namespace Common.FileStorage.Contracts
{
	using System.Threading.Tasks;

	public interface IFileStorage
	{
		Task<StorageFileInfo> CreateFileAsync(byte[] file);

		Task<StorageFileInfo> CreateFileAsync(string fileName, byte[] file);

		Task<StorageFileInfo> ReadFileAsync(string fileAddress);

		Task<StorageFileInfo> UpdateFileAsync(string existingFile, byte[] newFile);

		Task<StorageFileInfo> DeleteFileAsync(string existingFile);

		string GenerateFileName();
	}
}