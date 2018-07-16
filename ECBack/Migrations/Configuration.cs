namespace ECBack.Migrations
{
    using ECBack.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ECBack.Models.OracleDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ECBack.Models.OracleDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Categories.AddOrUpdate(x => x.CategoryID,
                new Models.Category() { CategoryID = 1, Name = "Novels" },
                new Models.Category() { CategoryID = 2, Name = "Toys" }
            );

            context.Users.AddOrUpdate(x => x.UserID,
                new User()
                {
                    UserID = 1,
                    PhoneNumber = "18817281365",
                    RealName = "mwish",
                    PasswordHash = "mwish",
                },
                new User()
                {
                    UserID = 2,
                    PhoneNumber = "15979161365",
                    RealName = "id<0",
                    PasswordHash = "id<0",
                });
        }
    }
}
