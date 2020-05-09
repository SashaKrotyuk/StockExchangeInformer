namespace SEI.News.Service.Hubs
{
	using Common.IoC.Autofac;
	using Microsoft.AspNet.SignalR;

	public class BaseHub : Hub
	{
		protected Common.IoC.Common.IDependencyResolver DependencyResolver
		{
			get
			{
				return AutofacService.Instance.Resolve<Common.IoC.Common.IDependencyResolver>();
			}
		}
	}
}