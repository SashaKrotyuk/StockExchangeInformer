namespace Common.IoC.Autofac
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using global::Autofac;
	using global::Common.IoC.Common;

	public class AutofacService : IDependencyResolver
	{
		private volatile bool initialized;
		private Dictionary<string, Assembly> assembliesWithModules;

		static AutofacService()
		{
			new AutofacService();
		}

		private AutofacService()
		{
			Instance = this;
			this.assembliesWithModules = new Dictionary<string, Assembly>();
		}

		public static AutofacService Instance { get; private set; }

		public IContainer Container { get; private set; }

		public bool Initialized => this.initialized;

		/// <summary>
		/// At the registration step these assemblies will be processed after those which are found automatically.
		/// Be careful! AddAssemblyWithModules - register of ALL components from the specified assembly!
		/// If you don not need all then registrate only those you need in newly created AutofacModule and
		/// add it in the executable assembly.
		/// </summary>
		/// <param name="asm">The assembly to discover registration modules.</param>
		public void AddAssemblyWithModules(Assembly asm)
		{
			lock (this.assembliesWithModules)
			{
				if (!this.assembliesWithModules.ContainsKey(asm.FullName))
				{
					lock (this.assembliesWithModules)
					{
						if (!this.assembliesWithModules.ContainsKey(asm.FullName))
						{
							this.assembliesWithModules.Add(asm.FullName, asm);
						}
					}
				}
			}
		}

		public void Initialize()
		{
			if (!this.initialized)
			{
				this.initialized = true;

				var builder = new ContainerBuilder();

				this.RegisterInternalModules(builder);

				this.Container = builder.Build();
			}
		}

		public T Resolve<T>(params DependencyParameter[] parameters)
		{
			return (T)Instance?.Container?.Resolve(typeof(T), parameters.Select(x => new NamedParameter(x.Name, x.Value)));
		}

		public T ResolveKeyed<T>(object key)
		{
			return (T)Instance?.Container?.ResolveKeyed(key, typeof(T));
		}

		public T ResolveOptionalKeyed<T>(object key) where T : class
		{
			return (T)Instance?.Container?.ResolveOptionalKeyed<T>(key);
		}

		private void RegisterInternalModules(ContainerBuilder builder)
		{
			var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
			var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
			var dotPos = assemblyName.IndexOf(".", StringComparison.Ordinal);
			var rootNamespace = dotPos == -1 ? assemblyName : assemblyName.Substring(0, dotPos);
			var internalAssemblies = new List<Assembly>();

			foreach (var assembly in domainAssemblies)
			{
				if (assembly.GetName().Name.StartsWith(rootNamespace))
				{
					if (!this.assembliesWithModules.ContainsKey(assembly.FullName))
					{
						internalAssemblies.Add(assembly);
					}
				}
			}

			Array.ForEach(this.assembliesWithModules.Values.ToArray(), internalAssemblies.Add);

			builder.RegisterAssemblyModules<AutofacModule>(internalAssemblies.ToArray());
		}
	}
}