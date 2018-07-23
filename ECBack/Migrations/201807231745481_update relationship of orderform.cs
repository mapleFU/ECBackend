namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updaterelationshipoforderform : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "DB2018.SaleEntityRecords", newName: "SERecords");
            AddColumn("DB2018.Orderforms", "SERecord_SaleEntityRecordID", c => c.Decimal(precision: 10, scale: 0));
            CreateIndex("DB2018.Orderforms", "SERecord_SaleEntityRecordID");
            AddForeignKey("DB2018.Orderforms", "SERecord_SaleEntityRecordID", "DB2018.SERecords", "SaleEntityRecordID");
        }
        
        public override void Down()
        {
            DropForeignKey("DB2018.Orderforms", "SERecord_SaleEntityRecordID", "DB2018.SERecords");
            DropIndex("DB2018.Orderforms", new[] { "SERecord_SaleEntityRecordID" });
            DropColumn("DB2018.Orderforms", "SERecord_SaleEntityRecordID");
            RenameTable(name: "DB2018.SERecords", newName: "SaleEntityRecords");
        }
    }
}
