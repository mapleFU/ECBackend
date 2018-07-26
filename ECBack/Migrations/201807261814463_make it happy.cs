namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makeithappy : DbMigration
    {
        public override void Up()
        {
            AlterColumn("DB2018.Addresses", "DetailAddress", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("DB2018.Addresses", "DetailAddress", c => c.String(nullable: false));
        }
    }
}
