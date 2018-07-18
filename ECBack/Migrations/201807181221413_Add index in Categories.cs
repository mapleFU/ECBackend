namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddindexinCategories : DbMigration
    {
        public override void Up()
        {
            CreateIndex("DB2018.Categories", "Name");
        }
        
        public override void Down()
        {
            DropIndex("DB2018.Categories", new[] { "Name" });
        }
    }
}
