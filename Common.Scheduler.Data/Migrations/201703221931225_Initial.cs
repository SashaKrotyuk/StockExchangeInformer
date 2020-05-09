namespace Common.Scheduler.Data.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Initial : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.ScheduledJobInfoes",
				c => new
				{
					Id = c.Guid(nullable: false),
					JobId = c.String(),
					CorrelationId = c.String(),
					ExecutionDate = c.DateTime(nullable: false, precision: 0, storeType: "datetime2"),
					IsDeleted = c.Boolean(nullable: false),
				})
				.PrimaryKey(t => t.Id);
		}

		public override void Down()
		{
			DropTable("dbo.ScheduledJobInfoes");
		}
	}
}