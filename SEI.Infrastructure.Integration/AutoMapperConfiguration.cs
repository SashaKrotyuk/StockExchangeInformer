namespace SEI.Infrastructure.Integration
{
	using AutoMapper;

	public static class AutoMapperConfiguration
	{
		public static void Configure()
		{
			Mapper.Initialize(
				config =>
				{
					////config.AddProfile(new {{profile}} ());
				});
		}
	}
}