namespace SEI.News.Model
{
	using System;
	using Common.Abstractions;

	public class NewsItem : ValueObject<NewsItem>
	{
		// required for EF
		protected NewsItem()
		{
		}

		public string Title { get; protected set; }

		public string Description { get; protected set; }

		public DateTime Date { get; protected set; }

		public string Timestamp { get; protected set; }

		public string Url { get; protected set; }

		public string ImageUrl { get; protected set; }

		public string SourceName { get; protected set; }

		public int UniqueIdentifier => this.GetHashCode();

		public static NewsItem New(
			string title,
			string description,
			DateTime date,
			string timestamp,
			string url,
			string imageUrl,
			string sourceName)
		{
			var result = new NewsItem
			{
				Title = title,
				Description = description,
				Date = date,
				Timestamp = timestamp,
				Url = url,
				ImageUrl = imageUrl,
				SourceName = sourceName
			};

			return result;
		}
	}
}