namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserandAddress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DB2018.Addresses",
                c => new
                    {
                        AddressID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        ReceiverName = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 50),
                        Province = c.String(nullable: false, maxLength: 20),
                        City = c.String(nullable: false, maxLength: 20),
                        Block = c.String(nullable: false, maxLength: 20),
                        DetailAddress = c.String(nullable: false),
                        IsDefault = c.Decimal(nullable: false, precision: 1, scale: 0),
                        UserID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.AddressID)
                .ForeignKey("DB2018.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "DB2018.Users",
                c => new
                    {
                        UserID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        NickName = c.String(nullable: false, maxLength: 50),
                        RealName = c.String(maxLength: 50),
                        PhoneNumber = c.String(nullable: false, maxLength: 11),
                        Gender = c.String(maxLength: 2000, fixedLength: true, unicode: false),
                        BirthDay = c.DateTime(nullable: false),
                        Local = c.String(maxLength: 100),
                        Home = c.String(maxLength: 100),
                        PasswordHash = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                .Index(t => t.NickName)
                .Index(t => t.PhoneNumber);
            
            CreateTable(
                "DB2018.Favorites",
                c => new
                    {
                        FavoriteID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        GoodEntityID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        UserID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.FavoriteID)
                .ForeignKey("DB2018.GoodEntities", t => t.GoodEntityID, cascadeDelete: true)
                .ForeignKey("DB2018.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.GoodEntityID)
                .Index(t => t.UserID);
            
            CreateTable(
                "DB2018.GoodEntities",
                c => new
                    {
                        GoodEntityID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        GoodName = c.String(nullable: false, maxLength: 100),
                        Brief = c.String(nullable: false, maxLength: 1000),
                        Detail = c.String(),
                        Stock = c.Decimal(nullable: false, precision: 10, scale: 0),
                        SellProvince = c.String(nullable: false),
                        GoodEntityState = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Category_CategoryID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.GoodEntityID)
                .ForeignKey("DB2018.Categories", t => t.Category_CategoryID)
                .Index(t => t.GoodName)
                .Index(t => t.Category_CategoryID);
            
            CreateTable(
                "DB2018.Orderforms",
                c => new
                    {
                        OrderformID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TransacDateTime = c.DateTime(nullable: false),
                        State = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TotalPrice = c.Single(nullable: false),
                        UserID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.OrderformID)
                .ForeignKey("DB2018.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
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
            
            CreateTable(
                "DB2018.Carts",
                c => new
                    {
                        UserID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("DB2018.Users", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "DB2018.Categories",
                c => new
                    {
                        CategoryID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CategoryID)
                .Index(t => t.Name);
            
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
            DropForeignKey("DB2018.LogisticInfoes", "LogisticID", "DB2018.Logistics");
            DropForeignKey("DB2018.GoodEntities", "Category_CategoryID", "DB2018.Categories");
            DropForeignKey("DB2018.Carts", "UserID", "DB2018.Users");
            DropForeignKey("DB2018.AttributeOptions", "GoodAttributeID", "DB2018.GoodAttributes");
            DropForeignKey("DB2018.Orderforms", "UserID", "DB2018.Users");
            DropForeignKey("DB2018.Favorites", "UserID", "DB2018.Users");
            DropForeignKey("DB2018.Favorites", "GoodEntityID", "DB2018.GoodEntities");
            DropForeignKey("DB2018.Addresses", "UserID", "DB2018.Users");
            DropIndex("DB2018.SaleEntities", new[] { "GoodEntityID" });
            DropIndex("DB2018.LogisticInfoes", new[] { "LogisticID" });
            DropIndex("DB2018.Categories", new[] { "Name" });
            DropIndex("DB2018.Carts", new[] { "UserID" });
            DropIndex("DB2018.AttributeOptions", new[] { "GoodAttributeID" });
            DropIndex("DB2018.Orderforms", new[] { "UserID" });
            DropIndex("DB2018.GoodEntities", new[] { "Category_CategoryID" });
            DropIndex("DB2018.GoodEntities", new[] { "GoodName" });
            DropIndex("DB2018.Favorites", new[] { "UserID" });
            DropIndex("DB2018.Favorites", new[] { "GoodEntityID" });
            DropIndex("DB2018.Users", new[] { "PhoneNumber" });
            DropIndex("DB2018.Users", new[] { "NickName" });
            DropIndex("DB2018.Addresses", new[] { "UserID" });
            DropTable("DB2018.SaleEntities");
            DropTable("DB2018.Logistics");
            DropTable("DB2018.LogisticInfoes");
            DropTable("DB2018.Categories");
            DropTable("DB2018.Carts");
            DropTable("DB2018.GoodAttributes");
            DropTable("DB2018.AttributeOptions");
            DropTable("DB2018.Orderforms");
            DropTable("DB2018.GoodEntities");
            DropTable("DB2018.Favorites");
            DropTable("DB2018.Users");
            DropTable("DB2018.Addresses");
        }
    }
}
