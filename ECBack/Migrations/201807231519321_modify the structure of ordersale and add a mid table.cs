namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifythestructureofordersaleandaddamidtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DB2018.SaleEntityRecords",
                c => new
                    {
                        SaleEntityRecordID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        EntityNum = c.Decimal(nullable: false, precision: 10, scale: 0),
                        SaleEntityID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.SaleEntityRecordID)
                .ForeignKey("DB2018.SaleEntities", t => t.SaleEntityID, cascadeDelete: true)
                .Index(t => t.SaleEntityID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("DB2018.SaleEntityRecords", "SaleEntityID", "DB2018.SaleEntities");
            DropIndex("DB2018.SaleEntityRecords", new[] { "SaleEntityID" });
            DropTable("DB2018.SaleEntityRecords");
        }
    }
}
