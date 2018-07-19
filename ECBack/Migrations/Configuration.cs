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
            context.Users.AddOrUpdate(u => u.UserID,
                new User()
                {
                    UserID = 1,
                    PhoneNumber = "18817281365",
                    NickName = "mwish",
                    PasswordHash = "mwish",
                    RealName = "空酱"
                },
                new User()
                {
                    UserID = 2,
                    PasswordHash = "???",
                    PhoneNumber = "18117167391",
                    NickName = "josh",
                    RealName = "sxa"
                });

            context.Addresses.AddOrUpdate(add => add.AddressID,
                new Address()
                {
                    UserID = 1,
                    Province = "上海",
                    City = "sHANGHAI",
                    Block = "黄渡镇男子职业技术学院",
                    DetailAddress = "ADJLKHFAKHDBK",
                    IsDefault = true,
                    ReceiverName = "mwish",
                    Phone = "18817281365"
                });
        }
    }
}
