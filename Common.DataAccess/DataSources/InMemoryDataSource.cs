namespace Common.DataAccess.DataSources
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
   
    using Common.Abstractions.Repository;
   
    public class InMemoryDataSource : IDataSource
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> ExecuteQueryResultList<T>(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public T ExecuteQueryResultSingle<T>(string query, int timeout = 30, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecuteQueryResultSingleAsync<T>(string query, int timeout = 30, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        void IDataSource.Add<TEntity, TId>(TEntity entity)
        {
            throw new NotImplementedException();
        }

        IQueryable<TEntity> IDataSource.GetResultList<TEntity>(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        IQueryable<TEntity> IDataSource.Set<TEntity>(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        TEntity IDataSource.GetResultSingle<TEntity>(Func<TEntity, bool> func, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        Task<TEntity> IDataSource.GetResultSingleAsync<TEntity>(Expression<Func<TEntity, bool>> func, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        void IDataSource.Remove<TEntity, TId>(TId id)
        {
            throw new NotImplementedException();
        }

        void IDataSource.Remove<TEntity, TId>(TEntity entity)
        {
            throw new NotImplementedException();
        }

        void IDataSource.Update<TEntity, TId>(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}