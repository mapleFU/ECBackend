namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatelogicforvip : DbMigration
    {
        public override void Up()
        {
            AddColumn("DB2018.Replies", "UserCommentTime", c => c.DateTime(nullable: false));
            AddColumn("DB2018.VIPs", "TotalCredits", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AddColumn("DB2018.VIPs", "AvailCredits", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            DropColumn("DB2018.Replies", "UserReplyTime");
        }
        
        public override void Down()
        {
            AddColumn("DB2018.Replies", "UserReplyTime", c => c.DateTime(nullable: false));
            DropColumn("DB2018.VIPs", "AvailCredits");
            DropColumn("DB2018.VIPs", "TotalCredits");
            DropColumn("DB2018.Replies", "UserCommentTime");
        }
    }
}
