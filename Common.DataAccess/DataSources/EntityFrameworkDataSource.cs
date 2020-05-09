namespace Common.DataAccess.DataSources
{
	using System;
	using System.Collections;
	using System.Data.Entity;
	using System.Data.Entity.Infrastructure;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	using Common.Abstractions;
	using Common.Abstractions.Entities;
	using Common.Abstractions.Repository;
	using Common.DataAccess.Extensions;

	public class EntityFrameworkDataSource : IDataSource
	{
		private readonly DbContext dbContext;
		private bool disposed;

		public EntityFrameworkDataSource(DbContext databaseContext)
		{
			this.dbContext = databaseContext;
			this.dbContext.Configuration.LazyLoadingEnabled = true;
		}

		public TEntity GetResultSingle<TEntity>(
			Func<TEntity, bool> func,
			params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IAggregateRoot
		{
			return this.dbContext.Set<TEntity>().AsNoTracking().IncludeMultiple(includeProperties).SingleOrDefault(func);
		}

		public async Task<TEntity> GetResultSingleAsync<TEntity>(
			Expression<Func<TEntity, bool>> func,
			params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IAggregateRoot
		{
			var result = await this.dbContext.Set<TEntity>().AsNoTracking().IncludeMultiple(includeProperties).SingleOrDefaultAsync(func);
			return result;
		}

		public IQueryable<TEntity> GetResultList<TEntity>(
			params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IAggregateRoot
		{
			return this.dbContext.Set<TEntity>().AsNoTracking().IncludeMultiple(includeProperties);
		}

		public void Add<TEntity, TId>(TEntity entity) where TEntity : class, IAggregateRoot
		{
			this.dbContext.Set(entity.GetType()).Add(entity);
			this.EnsureIdIsSet<TEntity, TId>(entity);
			SetCreationDate(entity);
			SetLastModificationDate(entity);
		}

		public void Remove<TEntity, TId>(TEntity entity)
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct
		{
			if (this.dbContext.Entry(entity).State == EntityState.Deleted)
			{
				return;
			}

			var entityId = this.GetKeyProperty<TEntity, TId>(entity).CurrentValue;
			var cachedEntity = this.dbContext.Set<TEntity>().Find(entityId);

			this.dbContext.Entry(cachedEntity ?? entity).State = EntityState.Deleted;
		}

		public void Remove<TEntity, TId>(TId id)
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct
		{
			var entity = this.dbContext.Set<TEntity>().Find(id);

			////if (entity == null)
			////{
			////	entity = new TEntity();
			////	var keyProperty = this.GetKeyProperty<TEntity, TId>(entity);
			////	keyProperty.CurrentValue = id;
			////}

			this.dbContext.Entry(entity).State = EntityState.Deleted;
		}

		public void Update<TEntity, TId>(TEntity entity)
			where TEntity : class, IHaveId<TId>, IAggregateRoot
			where TId : struct
		{
			try
			{
				////this.TraverseObject(entity);

				if (this.dbContext.Entry(entity).State == EntityState.Modified)
				{
					SetLastModificationDate(entity);
					this.FixState();
					return;
				}

				var entityId = this.GetKeyProperty<TEntity, TId>(entity).CurrentValue;
				var cachedEntity = this.dbContext.Set<TEntity>().Find(entityId);
				((IObjectContextAdapter)this.dbContext).ObjectContext.Detach(cachedEntity);

				SetLastModificationDate(entity);
				this.dbContext.Entry(entity).State = EntityState.Modified;
				this.FixState();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public T ExecuteQueryResultSingle<T>(string query, int timeout = 30, params object[] parameters)
		{
			var defaultTimeout = this.dbContext.Database.CommandTimeout;
			this.dbContext.Database.CommandTimeout = timeout;

			var result = this.dbContext.Database.SqlQuery<T>(query, parameters).SingleOrDefault();

			this.dbContext.Database.CommandTimeout = defaultTimeout;

			return result;
		}

		public async Task<T> ExecuteQueryResultSingleAsync<T>(string query, int timeout = 30, params object[] parameters)
		{
			var defaultTimeout = this.dbContext.Database.CommandTimeout;
			this.dbContext.Database.CommandTimeout = timeout;

			var result = this.dbContext.Database.SqlQuery<T>(query, parameters).SingleOrDefaultAsync();

			this.dbContext.Database.CommandTimeout = defaultTimeout;

			return await result;
		}

		public IQueryable<T> ExecuteQueryResultList<T>(string query, params object[] parameters)
		{
			return this.dbContext.Database.SqlQuery<T>(query, parameters).AsQueryable();
		}

		public int SaveChanges()
		{
			return this.dbContext.SaveChanges();
		}

		public async Task<int> SaveChangesAsync()
		{
			return await this.dbContext.SaveChangesAsync();
		}

		void IDisposable.Dispose()
		{
			if (!this.disposed)
			{
				this.dbContext?.Dispose();
			}

			this.disposed = true;
		}

		private void EnsureIdIsSet<TEntity, TId>(TEntity entity) where TEntity : class
		{
			var keyProperty = this.GetKeyProperty<TEntity, TId>(entity);

			if (keyProperty.CurrentValue is int)
			{
				return;
			}

			if (keyProperty.CurrentValue is Guid)
			{
				keyProperty.CurrentValue = (TId)(object)Guid.NewGuid();
			}
		}

		private void SetCreationDate<TEntity>(TEntity entity) where TEntity : class
		{
			var creationDateProperty = GetCreationDateProperty(entity);
			if (creationDateProperty != null)
			{
				creationDateProperty.CurrentValue = DateTime.UtcNow;
			}
		}

		private void SetLastModificationDate<TEntity>(TEntity entity) where TEntity : class
		{
			var lastModificationDateProperty = GetLastModificationDateProperty(entity);
			if (lastModificationDateProperty != null)
			{
				lastModificationDateProperty.CurrentValue = DateTime.UtcNow;
			}
		}

		private DbPropertyEntry<TEntity, TId> GetKeyProperty<TEntity, TId>(TEntity entity) where TEntity : class
		{
			try
			{
				var keyProperty = this.dbContext.Entry(entity).Property<TId>("Id");
				return keyProperty;
			}
			catch
			{
				// Property does not exists
				return null;
			}
		}

		private DbPropertyEntry<TEntity, DateTime> GetCreationDateProperty<TEntity>(TEntity entity) where TEntity : class
		{
			try
			{
				var keyProperty = this.dbContext.Entry(entity).Property<DateTime>("CreationDate");
				return keyProperty;
			}
			catch
			{
				// Property does not exists
				return null;
			}
		}

		private DbPropertyEntry<TEntity, DateTime> GetLastModificationDateProperty<TEntity>(TEntity entity) where TEntity : class
		{
			try
			{
				var keyProperty = this.dbContext.Entry(entity).Property<DateTime>("LastModificationDate");
				return keyProperty;
			}
			catch
			{
				// Property does not exists
				return null;
			}
		}

		private void SetTrackingState(IHaveTrackingState entity)
		{
			if (entity == null)
			{
				return;
			}

			try
			{
				var state = entity.TrackingState;

				if (state == TrackingState.Added)
				{
					this.dbContext.Entry(entity).State = EntityState.Added;
				}

				if (state == TrackingState.Modified)
				{
					this.dbContext.Entry(entity).State = EntityState.Modified;
				}

				if (state == TrackingState.Deleted)
				{
					this.dbContext.Entry(entity).State = EntityState.Deleted;
				}

				entity.TrackingState = TrackingState.Unchanged;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void TraverseObject<TEntity>(TEntity entity)
		{
			var entityProperties = entity.GetType().GetProperties();

			foreach (var property in entityProperties)
			{
				if (property.PropertyType.GetInterfaces().Contains(typeof(IHaveTrackingState)))
				{
					var value = (IHaveTrackingState)property.GetValue(entity);
					if (value != null)
					{
						this.SetTrackingState(value);
						this.TraverseObject(value);
					}
				}

				if (property.PropertyType.FullName != typeof(string).FullName
				 && property.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
				{
					var values = (IEnumerable)property.GetValue(entity);

					if (values != null)
					{
						foreach (var item in values)
						{
							if (item.GetType().GetInterfaces().Contains(typeof(IHaveTrackingState)))
							{
								this.SetTrackingState((IHaveTrackingState)item);
							}

							this.TraverseObject(item);
						}
					}
				}
			}
		}

		private void FixState()
		{
			foreach (var entry in this.dbContext.ChangeTracker.Entries<IHaveTrackingState>())
			{
				IHaveTrackingState stateInfo = entry.Entity;
				entry.State = this.ConvertState(stateInfo.TrackingState);
				stateInfo.TrackingState = TrackingState.Unchanged;
			}
		}

		private EntityState ConvertState(TrackingState state)
		{
			switch (state)
			{
				case TrackingState.Added:
					return EntityState.Added;
				case TrackingState.Modified:
					return EntityState.Modified;
				case TrackingState.Deleted:
					return EntityState.Deleted;
				default:
					return EntityState.Unchanged;
			}
		}
	}
}