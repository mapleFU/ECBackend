namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addreferencebetweenuserandvip : DbMigration
    {
        public override void Up()
        {
            AddColumn("DB2018.Users", "VIP_VIPID", c => c.Decimal(precision: 10, scale: 0));
            CreateIndex("DB2018.Users", "VIP_VIPID");
            AddForeignKey("DB2018.Users", "VIP_VIPID", "DB2018.VIPs", "VIPID");
        }
        
        public override void Down()
        {
            DropForeignKey("DB2018.Users", "VIP_VIPID", "DB2018.VIPs");
            DropIndex("DB2018.Users", new[] { "VIP_VIPID" });
            DropColumn("DB2018.Users", "VIP_VIPID");
        }
    }
}
