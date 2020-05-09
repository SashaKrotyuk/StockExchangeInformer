namespace Common.Scheduler.Data.Migrations
{
	using System;
	using System.Configuration;
	using System.Data.Entity;

	using Contracts;

	public class ScheduledJobsContext : DbContext
	{
		public ScheduledJobsContext()
			: base(nameOrConnectionString: ConnectionStringName)
		{
		}

		public static string ConnectionStringName
		{
			get
			{
				if (ConfigurationManager.ConnectionStrings["ScheduledJobsDB"]
					!= null)
				{
					return ConfigurationManager.ConnectionStrings["ScheduledJobsDB"].ToString();
				}

				return "DefaultConnection";
			}
		}

		public virtual DbSet<ScheduledJobInfo> ScheduledJobs { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2").HasPrecision(0));

			modelBuilder.Entity<ScheduledJobInfo>().HasKey(x => x.Id);

			base.OnModelCreating(modelBuilder);
		}
	}
}