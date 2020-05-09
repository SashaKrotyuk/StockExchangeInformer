namespace SEI.News.Data.Contracts
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using SEI.News.Model;

	public interface INewsRssSourceRepository : IDisposable
	{
		Task Add(NewsRssSource newsRssSource);

		Task DeleteById(Guid id);

		IEnumerable<NewsRssSource> GetAll();

		NewsRssSource GetById(Guid id);

		Task Update(NewsRssSource newsRssSource);
	}
}