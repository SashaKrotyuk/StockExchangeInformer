namespace Common.Tests.Infrastructure
{
	using Common.IoC.Autofac;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class IntegrationTestsAssemblyInitializer
	{
		[AssemblyInitialize]
		public static void AssemblyInitialize(TestContext context)
		{
			AutofacService.Instance.Initialize();
		}
	}
}