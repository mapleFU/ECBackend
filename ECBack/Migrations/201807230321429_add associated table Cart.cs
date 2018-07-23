namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addassociatedtableCart : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("DB2018.SaleEntities", "Cart_UserID", "DB2018.Carts");
            DropIndex("DB2018.SaleEntities", new[] { "Cart_UserID" });
            CreateTable(
                "DB2018.CartRecords",
                c => new
                    {
                        CartRecordID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        UserID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        SaleEntityID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        RecordNum = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.CartRecordID)
                .ForeignKey("DB2018.Carts", t => t.UserID, cascadeDelete: true)
                .ForeignKey("DB2018.SaleEntities", t => t.SaleEntityID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.SaleEntityID);
            
            DropColumn("DB2018.SaleEntities", "Cart_UserID");
        }
        
        public override void Down()
        {
            AddColumn("DB2018.SaleEntities", "Cart_UserID", c => c.Decimal(precision: 10, scale: 0));
            DropForeignKey("DB2018.CartRecords", "SaleEntityID", "DB2018.SaleEntities");
            DropForeignKey("DB2018.CartRecords", "UserID", "DB2018.Carts");
            DropIndex("DB2018.CartRecords", new[] { "SaleEntityID" });
            DropIndex("DB2018.CartRecords", new[] { "UserID" });
            DropTable("DB2018.CartRecords");
            CreateIndex("DB2018.SaleEntities", "Cart_UserID");
            AddForeignKey("DB2018.SaleEntities", "Cart_UserID", "DB2018.Carts", "UserID");
        }
    }
}
