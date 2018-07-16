namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addlogisticandinfoforthem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DB2018.LogisticInfoes",
                c => new
                    {
                        LogisticInfoId = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        LogisticID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Time = c.DateTime(nullable: false),
                        Position = c.String(),
                        State = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.LogisticInfoId)
                .ForeignKey("DB2018.Logistics", t => t.LogisticID, cascadeDelete: true)
                .Index(t => t.LogisticID);
            
            CreateTable(
                "DB2018.Logistics",
                c => new
                    {
                        LogisticID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        State = c.Decimal(nullable: false, precision: 10, scale: 0),
                        FromAddress = c.String(),
                        ToAddress = c.String(),
                    })
                .PrimaryKey(t => t.LogisticID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("DB2018.LogisticInfoes", "LogisticID", "DB2018.Logistics");
            DropIndex("DB2018.LogisticInfoes", new[] { "LogisticID" });
            DropTable("DB2018.Logistics");
            DropTable("DB2018.LogisticInfoes");
        }
    }
}
