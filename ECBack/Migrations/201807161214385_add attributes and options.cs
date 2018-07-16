namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addattributesandoptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DB2018.AttributeOptions",
                c => new
                    {
                        GoodAttributeID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Describe = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.GoodAttributeID)
                .ForeignKey("DB2018.GoodAttributes", t => t.GoodAttributeID)
                .Index(t => t.GoodAttributeID);
            
            CreateTable(
                "DB2018.GoodAttributes",
                c => new
                    {
                        GoodAttributeID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        GoodAttributeName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.GoodAttributeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("DB2018.AttributeOptions", "GoodAttributeID", "DB2018.GoodAttributes");
            DropIndex("DB2018.AttributeOptions", new[] { "GoodAttributeID" });
            DropTable("DB2018.GoodAttributes");
            DropTable("DB2018.AttributeOptions");
        }
    }
}
