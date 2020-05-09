namespace Common.DataAccess
{
	using System.Data.Entity;
	using System.Data.Entity.Migrations;

	public class DropCreateAndMigrateDatabaseInitializer<TContext, TMigrationsConfiguration>
		: IDatabaseInitializer<TContext>
		where TContext : DbContext
		where TMigrationsConfiguration : DbMigrationsConfiguration<TContext>, new()
	{
		public void InitializeDatabase(TContext context)
		{
			if (context.Database.Exists())
			{
				// set the database to SINGLE_USER so it can be dropped
				context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "ALTER DATABASE [" + context.Database.Connection.Database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");

				// drop the database
				context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "USE master DROP DATABASE [" + context.Database.Connection.Database + "]");
			}

			var migrator = new MigrateDatabaseToLatestVersion<TContext, TMigrationsConfiguration>();
			migrator.InitializeDatabase(context);
		}
	}
}