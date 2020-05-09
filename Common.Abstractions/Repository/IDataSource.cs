namespace Common.Abstractions.Repository
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	using Common.Abstractions.Entities;

	public interface IDataSource : IDisposable
	{
		TEntity GetResultSingle<TEntity>(Func<TEntity, bool> func, params Expression<Func<TEntity, object>>[] includeProperties)
			where TEntity : class, IAggregateRoot;

		Task<TEntity> GetResultSingleAsync<TEntity>(Expression<Func<TEntity, bool>> func, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IAggregateRoot;

		IQueryable<TEntity> GetResultList<TEntity>(params Expression<Func<TEntity, object>>[] includeProperties)
			where TEntity : class, IAggregateRoot;

		T ExecuteQueryResultSingle<T>(string query, int timeout = 30, params object[] parameters);

		IQueryable<T> ExecuteQueryResultList<T>(string query, params object[] parameters);

		Task<T> ExecuteQueryResultSingleAsync<T>(string query, int timeout = 30, params object[] parameters);

		void Add<TEntity, TId>(TEntity entity) where TEntity : class, IAggregateRoot;

		void Remove<TEntity, TId>(TEntity entity) 
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct;

		void Remove<TEntity, TId>(TId id)
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct;

		void Update<TEntity, TId>(TEntity entity)
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct;

		int SaveChanges();

		Task<int> SaveChangesAsync();
	}
}