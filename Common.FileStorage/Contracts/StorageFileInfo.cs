namespace Common.FileStorage.Contracts
{
	public class StorageFileInfo
	{
		public string FileName { get; set; }

		public string DownloadLink { get; set; }

		public string Location { get; set; }

		public byte[] Content { get; set; }
	}
}