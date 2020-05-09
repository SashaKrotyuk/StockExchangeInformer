namespace Common.DataAccess.Extensions
{
	using System;
	using System.Data.Entity;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;

	public static class QueryableExtensions
	{
		public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includeProperties) where T : class
		{
			if (includeProperties != null)
			{
				foreach (var includeProperty in includeProperties)
				{
					query = query.Include(includeProperty);
				}
			}

			return query;
		}

		public static IQueryable<TEntity> Order<TEntity>(this IQueryable<TEntity> source, SortDirections direction, string[] properties)
		{
			string o;

			switch (direction)
			{
				case SortDirections.ZtoA:
					o = "Descending";
					break;
				default:
					o = string.Empty;
					break;
			}

			var type = typeof(TEntity);
			var parameter = Expression.Parameter(type, "e");

			var order = "OrderBy" + o;
			var res = source;

			foreach (var p in properties)
			{
				Type propertyType;
				var propertyAccess = MakeMemberAccess(type, parameter, p, out propertyType);
				var orderByExp = Expression.Lambda(propertyAccess, parameter);
				var resultExp = Expression.Call(typeof(Queryable), order, new[] { type, propertyType }, res.Expression, Expression.Quote(orderByExp));

				res = res.Provider.CreateQuery<TEntity>(resultExp);

				order = "ThenBy" + o;
			}

			return res;
		}

		public static IQueryable<TEntity> Order<TEntity>(this IQueryable<TEntity> source, SortDirections direction, string ordering)
		{
			if (ordering.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
			{
				return source.Order(direction, ordering.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
			}

			string o;

			switch (direction)
			{
				case SortDirections.ZtoA:
					o = "OrderByDescending";
					break;
				default:
					o = "OrderBy";
					break;
			}

			var type = typeof(TEntity);
			var parameter = Expression.Parameter(type, "e");

			Type propertyType;
			var propertyAccess = MakeMemberAccess(type, parameter, ordering, out propertyType);
			var orderByExp = Expression.Lambda(propertyAccess, parameter);
			var resultExp = Expression.Call(typeof(Queryable), o, new[] { type, propertyType }, source.Expression, Expression.Quote(orderByExp));
			return source.Provider.CreateQuery<TEntity>(resultExp);
		}

		private static Expression MakeMemberAccess(Type type, Expression parameter, string propertyPath, out Type propertyType)
		{
			Expression expression = parameter;
			var currentType = type;

			foreach (var field in propertyPath.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries))
			{
				var property = currentType.GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
				expression = Expression.MakeMemberAccess(expression, property);
				currentType = property.PropertyType;
			}

			propertyType = currentType;

			return expression;
		}
	}
}