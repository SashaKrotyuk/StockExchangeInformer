namespace Common.Abstractions.Repository
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;

	using Common.Abstractions.Entities;

	public interface IGenericRepository : IDisposable
	{
		IQueryable<TEntity> GetAll<TEntity>(params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IAggregateRoot;
		
		IQueryable<TEntity> Find<TEntity>(
			Expression<Func<TEntity, bool>> predicate,
			params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IAggregateRoot;

		TEntity FindSingle<TEntity>(
			Expression<Func<TEntity, bool>> predicate,
			params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IAggregateRoot;

		void Add<TEntity, TId>(TEntity entity) where TEntity : class, IAggregateRoot;

		void AddAndSave<TEntity, TId>(TEntity entity) where TEntity : class, IAggregateRoot;

		void Update<TEntity, TId>(TEntity entity)
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct;

		void UpdateAndSave<TEntity, TId>(TEntity entity)
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct;

		void Delete<TEntity, TId>(TEntity entity)
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct;

		void DeleteAndSave<TEntity, TId>(TEntity entity)
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct;

		void DeleteById<TEntity, TId>(TId id)
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct;

		void DeleteByIdAndSave<TEntity, TId>(TId id)
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct;

		int SaveChanges();

		T ExecuteQueryResultSingle<T>(string query, int timeout = 30, params object[] parameters);

		IQueryable<T> ExecuteQueryResultList<T>(string query, params object[] parameters);
	}
}