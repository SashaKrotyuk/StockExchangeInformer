namespace Common.Events
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using Common.Abstractions;
	using Common.IoC.Autofac;
	using Common.IoC.Common;
	using MassTransit;

	/// <summary>
	/// Modified implementation of:
	/// http://msdn.microsoft.com/en-gb/magazine/ee236415.aspx#id0400046
	/// </summary>
	public static class DomainEvents
	{
		[ThreadStatic]
		private static List<Delegate> actions;

		static DomainEvents()
		{
			DependencyResolver = AutofacService.Instance?.Resolve<IDependencyResolver>();
		}

		public static IDependencyResolver DependencyResolver { get; set; }

		public static void Register<T>(Action<T> callback) where T : IDomainEvent
		{
			if (actions == null)
			{
				actions = new List<Delegate>();
			}

			actions.Add(callback);
		}

		public static void ClearCallbacks()
		{
			actions = null;
		}

		public static void Raise<T>(T args) where T : class, IDomainEvent
		{
			if (actions != null)
			{
				foreach (var action in actions)
				{
					if (action is Action<T>)
					{
						((Action<T>)action)(args);
					}
				}
			}

			Task.Run(async () => await DependencyResolver?.Resolve<IBus>()?.Publish(args));
		}
	}
}