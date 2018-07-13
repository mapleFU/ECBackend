namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "NEW_DB2018.Users",
                c => new
                    {
                        UserID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        NickName = c.String(maxLength: 50),
                        RealName = c.String(maxLength: 50),
                        PhoneNumber = c.String(maxLength: 11),
                        Gender = c.String(maxLength: 2000, fixedLength: true, unicode: false),
                        BirthDay = c.DateTime(nullable: false),
                        Local = c.String(maxLength: 100),
                        Home = c.String(maxLength: 100),
                        PasswordHash = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropTable("NEW_DB2018.Users");
        }
    }
}
