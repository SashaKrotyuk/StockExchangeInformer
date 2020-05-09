namespace SEI.News.Tests.NewsRssSource
{
	using Common.Abstractions.Entities;
	using NUnit.Framework;
	using SEI.News.Model;
	using Shouldly;

	[TestFixture]
	public class NewsRssSource_Should
	{
		[Test]
		public void BeCreated_UsingCreate_Method()
		{
			var result = NewsRssSource.Create(
				name: "Test",
				url: "Source",
				isEnabled: true);

			result.ShouldNotBeNull();
			result.Name.ShouldBe("Test");
			result.Url.ShouldBe("Source");
			result.IsEnabled.ShouldBeTrue();
		}

		[Test]
		public void BeAnAggregateRoot()
		{
			var result = NewsRssSource.Create(
				name: "Test",
				url: "Source",
				isEnabled: true);

			result.ShouldBeAssignableTo<IAggregateRoot>();
		}
	}
}
