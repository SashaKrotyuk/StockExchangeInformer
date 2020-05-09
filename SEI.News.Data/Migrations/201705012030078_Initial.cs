namespace SEI.News.Data.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class Initial : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.NewsRssSources",
				c => new
				{
					Id = c.Guid(nullable: false),
					CreationDate = c.DateTime(nullable: false, precision: 0, storeType: "datetime2"),
					LastModificationDate = c.DateTime(nullable: false, precision: 0, storeType: "datetime2"),
					IsEnabled = c.Boolean(nullable: false),
					Name = c.String(),
					Url = c.String(),
					IsDeleted = c.Boolean(nullable: false),
				})
				.PrimaryKey(t => t.Id);

		}

		public override void Down()
		{
			DropTable("dbo.NewsRssSources");
		}
	}
}