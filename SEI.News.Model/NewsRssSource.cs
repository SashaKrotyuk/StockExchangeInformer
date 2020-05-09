namespace SEI.News.Model
{
	using System;

	using Common.Abstractions;
	using Common.Abstractions.Entities;

	public class NewsRssSource : Entity<Guid>, IHaveHistory, IAggregateRoot
	{
		protected NewsRssSource()
			: base(Guid.NewGuid()) // required for EF
		{
		}

		public DateTime CreationDate { get; protected set; }

		public DateTime LastModificationDate { get; protected set; }

		public bool IsEnabled { get; set; }

		public string Name { get; set; }

		public string Url { get; set; }

		public static NewsRssSource Create(
			string name,
			string url,
			bool isEnabled)
		{
			var newSource = new NewsRssSource
			{
				Name = name,
				Url = url,
				CreationDate = DateTime.UtcNow,
				LastModificationDate = DateTime.UtcNow,
				IsEnabled = isEnabled
			};

			return newSource;
		}
	}
}