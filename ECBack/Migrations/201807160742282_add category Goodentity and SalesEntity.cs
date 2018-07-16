namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcategoryGoodentityandSalesEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DB2018.Categories",
                c => new
                    {
                        CategoryID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        Name = c.String(maxLength: 50),
                        GoodEntity_GoodEntityID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.CategoryID)
                .ForeignKey("DB2018.GoodEntities", t => t.GoodEntity_GoodEntityID)
                .Index(t => t.GoodEntity_GoodEntityID);
            
            CreateTable(
                "DB2018.GoodEntities",
                c => new
                    {
                        GoodEntityID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        GoodName = c.String(maxLength: 100),
                        Brief = c.String(maxLength: 1000),
                        Detail = c.String(),
                        Stock = c.Decimal(nullable: false, precision: 10, scale: 0),
                        SellProvince = c.String(),
                        GoodEntityState = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.GoodEntityID);
            
            CreateTable(
                "DB2018.SaleEntities",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GoodEntityID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Amount = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("DB2018.GoodEntities", t => t.GoodEntityID, cascadeDelete: true)
                .Index(t => t.GoodEntityID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("DB2018.SaleEntities", "GoodEntityID", "DB2018.GoodEntities");
            DropForeignKey("DB2018.Categories", "GoodEntity_GoodEntityID", "DB2018.GoodEntities");
            DropIndex("DB2018.SaleEntities", new[] { "GoodEntityID" });
            DropIndex("DB2018.Categories", new[] { "GoodEntity_GoodEntityID" });
            DropTable("DB2018.SaleEntities");
            DropTable("DB2018.GoodEntities");
            DropTable("DB2018.Categories");
        }
    }
}
