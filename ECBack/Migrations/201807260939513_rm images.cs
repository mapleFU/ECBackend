namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rmimages : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("DB2018.Images", "GoodEntity_GoodEntityID", "DB2018.GoodEntities");
            DropIndex("DB2018.Images", new[] { "GoodEntity_GoodEntityID" });
            DropColumn("DB2018.Images", "GoodEntity_GoodEntityID");
        }
        
        public override void Down()
        {
            AddColumn("DB2018.Images", "GoodEntity_GoodEntityID", c => c.Decimal(precision: 10, scale: 0));
            CreateIndex("DB2018.Images", "GoodEntity_GoodEntityID");
            AddForeignKey("DB2018.Images", "GoodEntity_GoodEntityID", "DB2018.GoodEntities", "GoodEntityID");
        }
    }
}
