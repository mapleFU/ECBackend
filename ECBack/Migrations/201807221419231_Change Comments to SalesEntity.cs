namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCommentstoSalesEntity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("DB2018.Comments", "GoodEntity_GoodEntityID", "DB2018.GoodEntities");
            DropIndex("DB2018.Comments", new[] { "GoodEntity_GoodEntityID" });
            DropIndex("DB2018.LogisticInfoes", new[] { "LogisticID" });
            AddColumn("DB2018.Comments", "SaleEntity_SaleEntityID", c => c.Decimal(precision: 10, scale: 0));
            AddColumn("DB2018.Orderforms", "AddressID", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            CreateIndex("DB2018.Comments", "SaleEntity_SaleEntityID");
            CreateIndex("DB2018.Orderforms", "AddressID");
            CreateIndex("DB2018.LogisticInfoes", "LogisticID");
            AddForeignKey("DB2018.Comments", "SaleEntity_SaleEntityID", "DB2018.SaleEntities", "SaleEntityID");
            AddForeignKey("DB2018.Orderforms", "AddressID", "DB2018.Addresses", "AddressID", cascadeDelete: true);
            DropColumn("DB2018.Comments", "GoodEntity_GoodEntityID");
        }
        
        public override void Down()
        {
            AddColumn("DB2018.Comments", "GoodEntity_GoodEntityID", c => c.Decimal(precision: 10, scale: 0));
            DropForeignKey("DB2018.Orderforms", "AddressID", "DB2018.Addresses");
            DropForeignKey("DB2018.Comments", "SaleEntity_SaleEntityID", "DB2018.SaleEntities");
            DropIndex("DB2018.LogisticInfoes", new[] { "LogisticID" });
            DropIndex("DB2018.Orderforms", new[] { "AddressID" });
            DropIndex("DB2018.Comments", new[] { "SaleEntity_SaleEntityID" });
            DropColumn("DB2018.Orderforms", "AddressID");
            DropColumn("DB2018.Comments", "SaleEntity_SaleEntityID");
            CreateIndex("DB2018.LogisticInfoes", "LogisticID");
            CreateIndex("DB2018.Comments", "GoodEntity_GoodEntityID");
            AddForeignKey("DB2018.Comments", "GoodEntity_GoodEntityID", "DB2018.GoodEntities", "GoodEntityID");
        }
    }
}
