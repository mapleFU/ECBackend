namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addinitforallmodels : DbMigration
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
                        Gender = c.String(maxLength: 2),
                        BirthDay = c.DateTime(nullable: false),
                        Local = c.String(maxLength: 100),
                        Home = c.String(maxLength: 100),
                        PasswordHash = c.String(nullable: false),
                        VIP_UserID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("DB2018.VIPs", t => t.VIP_UserID)
                .Index(t => t.NickName)
                .Index(t => t.PhoneNumber)
                .Index(t => t.VIP_UserID);
            
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
            
            CreateTable(
                "DB2018.SaleEntities",
                c => new
                    {
                        SaleEntityID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GoodEntityID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.SaleEntityID)
                .ForeignKey("DB2018.GoodEntities", t => t.GoodEntityID, cascadeDelete: true)
                .Index(t => t.GoodEntityID);
            
            CreateTable(
                "DB2018.Options",
                c => new
                    {
                        OptionID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        GAttributeID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Describe = c.String(nullable: false, maxLength: 20),
                        SaleEntity_SaleEntityID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.OptionID)
                .ForeignKey("DB2018.GAttributes", t => t.GAttributeID, cascadeDelete: true)
                .ForeignKey("DB2018.SaleEntities", t => t.SaleEntity_SaleEntityID)
                .Index(t => t.GAttributeID)
                .Index(t => t.SaleEntity_SaleEntityID);
            
            CreateTable(
                "DB2018.GAttributes",
                c => new
                    {
                        GAttributeID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        GAttributeName = c.String(nullable: false, maxLength: 100),
                        GoodEntity_GoodEntityID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.GAttributeID)
                .ForeignKey("DB2018.GoodEntities", t => t.GoodEntity_GoodEntityID)
                .Index(t => t.GoodEntity_GoodEntityID);
            
            CreateTable(
                "DB2018.Comments",
                c => new
                    {
                        CommentID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        Detail = c.String(maxLength: 400),
                        LevelRank = c.Decimal(nullable: false, precision: 10, scale: 0),
                        UserCommentTime = c.DateTime(nullable: false),
                        UserID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        SaleEntityID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("DB2018.SaleEntities", t => t.SaleEntityID, cascadeDelete: true)
                .Index(t => t.SaleEntityID);
            
            CreateTable(
                "DB2018.Coupons",
                c => new
                    {
                        CouponID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        Min = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Decrease = c.Decimal(nullable: false, precision: 10, scale: 0),
                        NeedVIP = c.Decimal(nullable: false, precision: 1, scale: 0),
                        CategoryID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.CouponID)
                .ForeignKey("DB2018.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
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
                "DB2018.GoodEntities",
                c => new
                    {
                        GoodEntityID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        SellerID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        GoodName = c.String(nullable: false, maxLength: 100),
                        Brief = c.String(nullable: false, maxLength: 1000),
                        Detail = c.String(),
                        DetailImages = c.String(),
                        Stock = c.Decimal(nullable: false, precision: 10, scale: 0),
                        SellProvince = c.String(nullable: false),
                        GoodEntityState = c.Decimal(nullable: false, precision: 10, scale: 0),
                        FavoriteNum = c.Decimal(nullable: false, precision: 10, scale: 0),
                        BrandID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.GoodEntityID)
                .ForeignKey("DB2018.Brands", t => t.BrandID, cascadeDelete: true)
                .ForeignKey("DB2018.Sellers", t => t.SellerID, cascadeDelete: true)
                .Index(t => t.SellerID)
                .Index(t => t.GoodName)
                .Index(t => t.BrandID);
            
            CreateTable(
                "DB2018.Brands",
                c => new
                    {
                        BrandID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        BrandName = c.String(nullable: false, maxLength: 50),
                        Detail = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.BrandID)
                .Index(t => t.BrandName);
            
            CreateTable(
                "DB2018.Images",
                c => new
                    {
                        ImageID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        ImageURL = c.String(nullable: false),
                        Local = c.Decimal(nullable: false, precision: 1, scale: 0),
                        GoodEntity_GoodEntityID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ImageID)
                .ForeignKey("DB2018.GoodEntities", t => t.GoodEntity_GoodEntityID)
                .Index(t => t.GoodEntity_GoodEntityID);
            
            CreateTable(
                "DB2018.Questions",
                c => new
                    {
                        QuestionID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        Detail = c.String(maxLength: 50),
                        GoodEntityID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.QuestionID)
                .ForeignKey("DB2018.GoodEntities", t => t.GoodEntityID, cascadeDelete: true)
                .Index(t => t.GoodEntityID);
            
            CreateTable(
                "DB2018.Replies",
                c => new
                    {
                        ReplyID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        ReplyDetail = c.String(maxLength: 200),
                        UserReplyTime = c.DateTime(nullable: false),
                        QuestionID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ReplyID)
                .ForeignKey("DB2018.Questions", t => t.QuestionID, cascadeDelete: true)
                .Index(t => t.QuestionID);
            
            CreateTable(
                "DB2018.Sellers",
                c => new
                    {
                        SellerID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        PhoneNumber = c.String(),
                        PasswordHash = c.String(),
                    })
                .PrimaryKey(t => t.SellerID);
            
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
                "DB2018.Orderforms",
                c => new
                    {
                        OrderformID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        TransacDateTime = c.DateTime(nullable: false),
                        State = c.Decimal(nullable: false, precision: 10, scale: 0),
                        TotalPrice = c.Single(nullable: false),
                        UserID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        AddressID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.OrderformID)
                .ForeignKey("DB2018.Addresses", t => t.AddressID, cascadeDelete: true)
                .ForeignKey("DB2018.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.AddressID);
            
            CreateTable(
                "DB2018.Logistics",
                c => new
                    {
                        LogisticID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        State = c.Decimal(nullable: false, precision: 10, scale: 0),
                        FromAddress = c.String(),
                        ToAddress = c.String(),
                    })
                .PrimaryKey(t => t.LogisticID)
                .ForeignKey("DB2018.Orderforms", t => t.LogisticID)
                .Index(t => t.LogisticID);
            
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
                "DB2018.SERecords",
                c => new
                    {
                        SaleEntityRecordID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        EntityNum = c.Decimal(nullable: false, precision: 10, scale: 0),
                        SaleEntityID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        OrderformID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.SaleEntityRecordID)
                .ForeignKey("DB2018.Orderforms", t => t.OrderformID, cascadeDelete: true)
                .ForeignKey("DB2018.SaleEntities", t => t.SaleEntityID, cascadeDelete: true)
                .Index(t => t.SaleEntityID)
                .Index(t => t.OrderformID);
            
            CreateTable(
                "DB2018.VIPs",
                c => new
                    {
                        UserID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        TotalCredits = c.Decimal(nullable: false, precision: 10, scale: 0),
                        AvailCredits = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "DB2018.GoodEntityCategories",
                c => new
                    {
                        GoodEntity_GoodEntityID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Category_CategoryID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.GoodEntity_GoodEntityID, t.Category_CategoryID })
                .ForeignKey("DB2018.GoodEntities", t => t.GoodEntity_GoodEntityID, cascadeDelete: true)
                .ForeignKey("DB2018.Categories", t => t.Category_CategoryID, cascadeDelete: true)
                .Index(t => t.GoodEntity_GoodEntityID)
                .Index(t => t.Category_CategoryID);
            
            CreateTable(
                "DB2018.CouponsUsers",
                c => new
                    {
                        Coupons_CouponID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        User_UserID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.Coupons_CouponID, t.User_UserID })
                .ForeignKey("DB2018.Coupons", t => t.Coupons_CouponID, cascadeDelete: true)
                .ForeignKey("DB2018.Users", t => t.User_UserID, cascadeDelete: true)
                .Index(t => t.Coupons_CouponID)
                .Index(t => t.User_UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("DB2018.Users", "VIP_UserID", "DB2018.VIPs");
            DropForeignKey("DB2018.SERecords", "SaleEntityID", "DB2018.SaleEntities");
            DropForeignKey("DB2018.SERecords", "OrderformID", "DB2018.Orderforms");
            DropForeignKey("DB2018.Logistics", "LogisticID", "DB2018.Orderforms");
            DropForeignKey("DB2018.LogisticInfoes", "LogisticID", "DB2018.Logistics");
            DropForeignKey("DB2018.Orderforms", "UserID", "DB2018.Users");
            DropForeignKey("DB2018.Orderforms", "AddressID", "DB2018.Addresses");
            DropForeignKey("DB2018.Favorites", "UserID", "DB2018.Users");
            DropForeignKey("DB2018.Favorites", "GoodEntityID", "DB2018.GoodEntities");
            DropForeignKey("DB2018.CouponsUsers", "User_UserID", "DB2018.Users");
            DropForeignKey("DB2018.CouponsUsers", "Coupons_CouponID", "DB2018.Coupons");
            DropForeignKey("DB2018.GoodEntities", "SellerID", "DB2018.Sellers");
            DropForeignKey("DB2018.SaleEntities", "GoodEntityID", "DB2018.GoodEntities");
            DropForeignKey("DB2018.Replies", "QuestionID", "DB2018.Questions");
            DropForeignKey("DB2018.Questions", "GoodEntityID", "DB2018.GoodEntities");
            DropForeignKey("DB2018.Images", "GoodEntity_GoodEntityID", "DB2018.GoodEntities");
            DropForeignKey("DB2018.GAttributes", "GoodEntity_GoodEntityID", "DB2018.GoodEntities");
            DropForeignKey("DB2018.GoodEntityCategories", "Category_CategoryID", "DB2018.Categories");
            DropForeignKey("DB2018.GoodEntityCategories", "GoodEntity_GoodEntityID", "DB2018.GoodEntities");
            DropForeignKey("DB2018.GoodEntities", "BrandID", "DB2018.Brands");
            DropForeignKey("DB2018.Coupons", "CategoryID", "DB2018.Categories");
            DropForeignKey("DB2018.Carts", "UserID", "DB2018.Users");
            DropForeignKey("DB2018.CartRecords", "SaleEntityID", "DB2018.SaleEntities");
            DropForeignKey("DB2018.Comments", "SaleEntityID", "DB2018.SaleEntities");
            DropForeignKey("DB2018.Options", "SaleEntity_SaleEntityID", "DB2018.SaleEntities");
            DropForeignKey("DB2018.Options", "GAttributeID", "DB2018.GAttributes");
            DropForeignKey("DB2018.CartRecords", "UserID", "DB2018.Carts");
            DropForeignKey("DB2018.Addresses", "UserID", "DB2018.Users");
            DropIndex("DB2018.CouponsUsers", new[] { "User_UserID" });
            DropIndex("DB2018.CouponsUsers", new[] { "Coupons_CouponID" });
            DropIndex("DB2018.GoodEntityCategories", new[] { "Category_CategoryID" });
            DropIndex("DB2018.GoodEntityCategories", new[] { "GoodEntity_GoodEntityID" });
            DropIndex("DB2018.SERecords", new[] { "OrderformID" });
            DropIndex("DB2018.SERecords", new[] { "SaleEntityID" });
            DropIndex("DB2018.LogisticInfoes", new[] { "LogisticID" });
            DropIndex("DB2018.Logistics", new[] { "LogisticID" });
            DropIndex("DB2018.Orderforms", new[] { "AddressID" });
            DropIndex("DB2018.Orderforms", new[] { "UserID" });
            DropIndex("DB2018.Favorites", new[] { "UserID" });
            DropIndex("DB2018.Favorites", new[] { "GoodEntityID" });
            DropIndex("DB2018.Replies", new[] { "QuestionID" });
            DropIndex("DB2018.Questions", new[] { "GoodEntityID" });
            DropIndex("DB2018.Images", new[] { "GoodEntity_GoodEntityID" });
            DropIndex("DB2018.Brands", new[] { "BrandName" });
            DropIndex("DB2018.GoodEntities", new[] { "BrandID" });
            DropIndex("DB2018.GoodEntities", new[] { "GoodName" });
            DropIndex("DB2018.GoodEntities", new[] { "SellerID" });
            DropIndex("DB2018.Categories", new[] { "Name" });
            DropIndex("DB2018.Coupons", new[] { "CategoryID" });
            DropIndex("DB2018.Comments", new[] { "SaleEntityID" });
            DropIndex("DB2018.GAttributes", new[] { "GoodEntity_GoodEntityID" });
            DropIndex("DB2018.Options", new[] { "SaleEntity_SaleEntityID" });
            DropIndex("DB2018.Options", new[] { "GAttributeID" });
            DropIndex("DB2018.SaleEntities", new[] { "GoodEntityID" });
            DropIndex("DB2018.CartRecords", new[] { "SaleEntityID" });
            DropIndex("DB2018.CartRecords", new[] { "UserID" });
            DropIndex("DB2018.Carts", new[] { "UserID" });
            DropIndex("DB2018.Users", new[] { "VIP_UserID" });
            DropIndex("DB2018.Users", new[] { "PhoneNumber" });
            DropIndex("DB2018.Users", new[] { "NickName" });
            DropIndex("DB2018.Addresses", new[] { "UserID" });
            DropTable("DB2018.CouponsUsers");
            DropTable("DB2018.GoodEntityCategories");
            DropTable("DB2018.VIPs");
            DropTable("DB2018.SERecords");
            DropTable("DB2018.LogisticInfoes");
            DropTable("DB2018.Logistics");
            DropTable("DB2018.Orderforms");
            DropTable("DB2018.Favorites");
            DropTable("DB2018.Sellers");
            DropTable("DB2018.Replies");
            DropTable("DB2018.Questions");
            DropTable("DB2018.Images");
            DropTable("DB2018.Brands");
            DropTable("DB2018.GoodEntities");
            DropTable("DB2018.Categories");
            DropTable("DB2018.Coupons");
            DropTable("DB2018.Comments");
            DropTable("DB2018.GAttributes");
            DropTable("DB2018.Options");
            DropTable("DB2018.SaleEntities");
            DropTable("DB2018.CartRecords");
            DropTable("DB2018.Carts");
            DropTable("DB2018.Users");
            DropTable("DB2018.Addresses");
        }
    }
}
