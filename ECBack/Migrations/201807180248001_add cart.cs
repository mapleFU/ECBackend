namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DB2018.Carts",
                c => new
                    {
                        UserID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("DB2018.Users", t => t.UserID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("DB2018.Carts", "UserID", "DB2018.Users");
            DropIndex("DB2018.Carts", new[] { "UserID" });
            DropTable("DB2018.Carts");
        }
    }
}
