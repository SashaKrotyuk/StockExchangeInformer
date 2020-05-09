namespace Common.Abstractions.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	using Common.Abstractions.Entities;

	public interface IAsyncGenericRepository : IGenericRepository
	{
		Task<IEnumerable<TEntity>> GetByComplexExpressionAsync<TEntity>(
			Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> expression,
			params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IAggregateRoot;

		Task<IEnumerable<TOut>> GetByComplexExpressionAndModifyAsync<TEntity, TOut>(
			Expression<Func<IQueryable<TEntity>, IQueryable<TOut>>> expression,
			params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IAggregateRoot;

		Task<TEntity> FindSingleAsync<TEntity>(
			Expression<Func<TEntity, bool>> predicate,
			params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IAggregateRoot;

		Task AddAndSaveAsync<TEntity, TId>(TEntity entity) where TEntity : class, IAggregateRoot;

		Task UpdateAndSaveAsync<TEntity, TId>(TEntity entity)
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct;

		Task DeleteAndSaveAsync<TEntity, TId>(TEntity entity)
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct;

		Task DeleteByIdAndSaveAsync<TEntity, TId>(TId id)
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct;

		Task<int> SaveChangesAsync();
	}
}