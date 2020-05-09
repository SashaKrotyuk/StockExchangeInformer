namespace Common.DataAccess.DataSources
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	using Common.Abstractions.Entities;
	using Common.Abstractions.Repository;
	using Common.DataAccess.Extensions;
	using MongoDB.Driver;
	using MongoDB.Driver.Linq;

	public class MongoDbDataSource : IDataSource
	{
		private IMongoDatabase _database;

		public MongoDbDataSource(IMongoDatabase database)
		{
			this._database = database;
		}

		public TEntity GetResultSingle<TEntity>(
			Func<TEntity, bool> func,
			params Expression<Func<TEntity, object>>[] includeProperties)
			where TEntity : class, IAggregateRoot
		{
			return this._database.GetCollection<TEntity>(typeof(TEntity).Name).AsQueryable().SingleOrDefault(func);
		}

		public async Task<TEntity> GetResultSingleAsync<TEntity>(
			Expression<Func<TEntity, bool>> func,
			params Expression<Func<TEntity, object>>[] includeProperties)
			where TEntity : class, IAggregateRoot
		{
			var result = await this._database.GetCollection<TEntity>(typeof(TEntity).Name).AsQueryable().SingleOrDefaultAsync(func);
			return result;
		}

		public IQueryable<TEntity> GetResultList<TEntity>(
			params Expression<Func<TEntity, object>>[] includeProperties)
			where TEntity : class, IAggregateRoot
		{
			return this._database.GetCollection<TEntity>(typeof(TEntity).Name).AsQueryable();
		}

		public IQueryable<TEntity> Set<TEntity>(
			params Expression<Func<TEntity, object>>[] includeProperties)
			where TEntity : class, IAggregateRoot
		{
			return this._database.GetCollection<TEntity>(typeof(TEntity).Name).AsQueryable();
		}

		public void Add<TEntity, TId>(TEntity entity)
			where TEntity : class, IAggregateRoot
		{
			this._database.GetCollection<TEntity>(typeof(TEntity).Name).InsertOne(entity);
		}

		public void Remove<TEntity, TId>(TEntity entity)
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct
		{
			var filter = Builders<TEntity>.Filter.Eq("_id", entity.Id);
			this._database.GetCollection<TEntity>(typeof(TEntity).Name).DeleteOne(filter);
		}

		public void Remove<TEntity, TId>(TId id)
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct
		{
			var filter = Builders<TEntity>.Filter.Eq("_id", id);
			this._database.GetCollection<TEntity>(typeof(TEntity).Name).DeleteOne(filter);
		}

		public void Update<TEntity, TId>(TEntity entity)
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct
		{
			var filter = Builders<TEntity>.Filter.Eq("_id", entity.Id);
			this._database.GetCollection<TEntity>(typeof(TEntity).Name).ReplaceOne(filter, entity);
		}

		public T ExecuteQueryResultSingle<T>(string query, int timeout = 30, params object[] parameters)
		{
			throw new NotImplementedException();
		}

		public Task<T> ExecuteQueryResultSingleAsync<T>(string query, int timeout = 30, params object[] parameters)
		{
			throw new NotImplementedException();
		}

		public IQueryable<T> ExecuteQueryResultList<T>(string query, params object[] parameters)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
		}

		public int SaveChanges()
		{
			return 1;
		}

		public Task<int> SaveChangesAsync()
		{
			return Task.FromResult(1);
		}
	}
}