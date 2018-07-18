namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateController : DbMigration
    {
        public override void Up()
        {
            AlterColumn("DB2018.Users", "NickName", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("DB2018.Users", "NickName");
        }
        
        public override void Down()
        {
            DropIndex("DB2018.Users", new[] { "NickName" });
            AlterColumn("DB2018.Users", "NickName", c => c.String(maxLength: 50));
        }
    }
}
