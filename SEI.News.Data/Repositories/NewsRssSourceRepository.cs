namespace SEI.News.Data.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using Common.Abstractions.Repository;
	using Contracts;
	using SEI.News.Model;
	
	public class NewsRssSourceRepository : INewsRssSourceRepository
	{
		private readonly IAsyncGenericRepository _repository;

		public NewsRssSourceRepository(IAsyncGenericRepository genericRepository)
		{
			this._repository = genericRepository;
		}

		public Task Add(NewsRssSource newsRssSource)
		{
			return this._repository.AddAndSaveAsync<NewsRssSource, Guid>(newsRssSource);
		}

		public IEnumerable<NewsRssSource> GetAll()
		{
			return this._repository.GetAll<NewsRssSource>().ToList();
		}

		public NewsRssSource GetById(Guid id)
		{
			return this._repository.FindSingle<NewsRssSource>(rssSource => rssSource.Id == id);
		}

		public Task Update(NewsRssSource newsRssSource)
		{
			return this._repository.UpdateAndSaveAsync<NewsRssSource, Guid>(newsRssSource);
		}

		public Task DeleteById(Guid id)
		{
			return this._repository.DeleteByIdAndSaveAsync<NewsRssSource, Guid>(id);
		}

		public void Dispose()
		{
			this._repository.Dispose();
		}
	}
}