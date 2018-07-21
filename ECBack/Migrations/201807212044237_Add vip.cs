namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addvip : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DB2018.VIPs",
                c => new
                    {
                        VIPID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.VIPID);
            
        }
        
        public override void Down()
        {
            DropTable("DB2018.VIPs");
        }
    }
}
