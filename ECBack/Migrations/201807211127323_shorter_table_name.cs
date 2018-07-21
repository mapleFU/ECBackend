namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shorter_table_name : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("DB2018.Coupons", "User_UserID", "DB2018.Users");
            DropForeignKey("DB2018.AttributeOptions", "GoodAttributeID", "DB2018.GoodAttributes");
            DropForeignKey("DB2018.CommentInfoes", "CommentID", "DB2018.Comments");
            DropForeignKey("DB2018.Comments", "SaleEntityID", "DB2018.SaleEntities");
            DropForeignKey("DB2018.LogisticInfoes", "LogisticID", "DB2018.Logistics");
            DropIndex("DB2018.Coupons", new[] { "User_UserID" });
            DropIndex("DB2018.AttributeOptions", new[] { "GoodAttributeID" });
            DropIndex("DB2018.CommentInfoes", new[] { "CommentID" });
            DropIndex("DB2018.Comments", new[] { "SaleEntityID" });
            DropPrimaryKey("DB2018.Coupons");
            DropPrimaryKey("DB2018.Logistics");
            CreateTable(
                "DB2018.Options",
                c => new
                    {
                        OptionID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        GAttributeID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Describe = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.OptionID)
                .ForeignKey("DB2018.GAttributes", t => t.GAttributeID, cascadeDelete: true)
                .Index(t => t.GAttributeID);
            
            CreateTable(
                "DB2018.GAttributes",
                c => new
                    {
                        GAttributeID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        GAttributeName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.GAttributeID);
            
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
                "DB2018.OptionGoodEntities",
                c => new
                    {
                        Option_OptionID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        GoodEntity_GoodEntityID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.Option_OptionID, t.GoodEntity_GoodEntityID })
                .ForeignKey("DB2018.Options", t => t.Option_OptionID, cascadeDelete: true)
                .ForeignKey("DB2018.GoodEntities", t => t.GoodEntity_GoodEntityID, cascadeDelete: true)
                .Index(t => t.Option_OptionID)
                .Index(t => t.GoodEntity_GoodEntityID);
            
            CreateTable(
                "DB2018.SaleEntityOptions",
                c => new
                    {
                        SaleEntity_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Option_OptionID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.SaleEntity_ID, t.Option_OptionID })
                .ForeignKey("DB2018.SaleEntities", t => t.SaleEntity_ID, cascadeDelete: true)
                .ForeignKey("DB2018.Options", t => t.Option_OptionID, cascadeDelete: true)
                .Index(t => t.SaleEntity_ID)
                .Index(t => t.Option_OptionID);
            
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
            
            AddColumn("DB2018.Coupons", "CouponID", c => c.Decimal(nullable: false, precision: 10, scale: 0, identity: true));
            AddColumn("DB2018.GoodEntities", "BrandID", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AddColumn("DB2018.Comments", "DisplayEntityID", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AddColumn("DB2018.Comments", "Detail", c => c.String(maxLength: 400));
            AddColumn("DB2018.Comments", "LevelRank", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AddColumn("DB2018.Comments", "UserCommentTime", c => c.DateTime(nullable: false));
            AddColumn("DB2018.SaleEntities", "InCart", c => c.Decimal(nullable: false, precision: 1, scale: 0));
            AddColumn("DB2018.SaleEntities", "UserID", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AddColumn("DB2018.SaleEntities", "Orderform_OrderformID", c => c.Decimal(precision: 10, scale: 0));
            AlterColumn("DB2018.Coupons", "NeedVIP", c => c.Decimal(nullable: false, precision: 1, scale: 0));
            AlterColumn("DB2018.Logistics", "LogisticID", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AddPrimaryKey("DB2018.Coupons", "CouponID");
            AddPrimaryKey("DB2018.Logistics", "LogisticID");
            CreateIndex("DB2018.GoodEntities", "BrandID");
            CreateIndex("DB2018.SaleEntities", "UserID");
            CreateIndex("DB2018.SaleEntities", "Orderform_OrderformID");
            CreateIndex("DB2018.Comments", "DisplayEntityID");
            CreateIndex("DB2018.Logistics", "LogisticID");
            AddForeignKey("DB2018.Comments", "DisplayEntityID", "DB2018.DisplayEntities", "ID", cascadeDelete: true);
            AddForeignKey("DB2018.SaleEntities", "UserID", "DB2018.Users", "UserID", cascadeDelete: true);
            AddForeignKey("DB2018.GoodEntities", "BrandID", "DB2018.Brands", "BrandID", cascadeDelete: true);
            AddForeignKey("DB2018.Logistics", "LogisticID", "DB2018.Orderforms", "OrderformID");
            AddForeignKey("DB2018.SaleEntities", "Orderform_OrderformID", "DB2018.Orderforms", "OrderformID");
            AddForeignKey("DB2018.SaleEntities", "UserID", "DB2018.Carts", "UserID", cascadeDelete: true);
            AddForeignKey("DB2018.LogisticInfoes", "LogisticID", "DB2018.Logistics", "LogisticID", cascadeDelete: true);
            DropColumn("DB2018.Coupons", "ID");
            DropColumn("DB2018.Coupons", "User_UserID");
            DropColumn("DB2018.Comments", "SaleEntityID");
            DropTable("DB2018.AttributeOptions");
            DropTable("DB2018.GoodAttributes");
            DropTable("DB2018.CommentInfoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "DB2018.CommentInfoes",
                c => new
                    {
                        CommentID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Detail = c.String(maxLength: 400),
                        LevelRank = c.Decimal(nullable: false, precision: 10, scale: 0),
                        UserCommentTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommentID);
            
            CreateTable(
                "DB2018.GoodAttributes",
                c => new
                    {
                        GoodAttributeID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        GoodAttributeName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.GoodAttributeID);
            
            CreateTable(
                "DB2018.AttributeOptions",
                c => new
                    {
                        GoodAttributeID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Describe = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.GoodAttributeID);
            
            AddColumn("DB2018.Comments", "SaleEntityID", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AddColumn("DB2018.Coupons", "User_UserID", c => c.Decimal(precision: 10, scale: 0));
            AddColumn("DB2018.Coupons", "ID", c => c.Decimal(nullable: false, precision: 10, scale: 0, identity: true));
            DropForeignKey("DB2018.LogisticInfoes", "LogisticID", "DB2018.Logistics");
            DropForeignKey("DB2018.SaleEntities", "UserID", "DB2018.Carts");
            DropForeignKey("DB2018.SaleEntities", "Orderform_OrderformID", "DB2018.Orderforms");
            DropForeignKey("DB2018.Logistics", "LogisticID", "DB2018.Orderforms");
            DropForeignKey("DB2018.CouponsUsers", "User_UserID", "DB2018.Users");
            DropForeignKey("DB2018.CouponsUsers", "Coupons_CouponID", "DB2018.Coupons");
            DropForeignKey("DB2018.GoodEntities", "BrandID", "DB2018.Brands");
            DropForeignKey("DB2018.SaleEntities", "UserID", "DB2018.Users");
            DropForeignKey("DB2018.Comments", "DisplayEntityID", "DB2018.DisplayEntities");
            DropForeignKey("DB2018.SaleEntityOptions", "Option_OptionID", "DB2018.Options");
            DropForeignKey("DB2018.SaleEntityOptions", "SaleEntity_ID", "DB2018.SaleEntities");
            DropForeignKey("DB2018.OptionGoodEntities", "GoodEntity_GoodEntityID", "DB2018.GoodEntities");
            DropForeignKey("DB2018.OptionGoodEntities", "Option_OptionID", "DB2018.Options");
            DropForeignKey("DB2018.Options", "GAttributeID", "DB2018.GAttributes");
            DropIndex("DB2018.CouponsUsers", new[] { "User_UserID" });
            DropIndex("DB2018.CouponsUsers", new[] { "Coupons_CouponID" });
            DropIndex("DB2018.SaleEntityOptions", new[] { "Option_OptionID" });
            DropIndex("DB2018.SaleEntityOptions", new[] { "SaleEntity_ID" });
            DropIndex("DB2018.OptionGoodEntities", new[] { "GoodEntity_GoodEntityID" });
            DropIndex("DB2018.OptionGoodEntities", new[] { "Option_OptionID" });
            DropIndex("DB2018.Logistics", new[] { "LogisticID" });
            DropIndex("DB2018.Brands", new[] { "BrandName" });
            DropIndex("DB2018.Comments", new[] { "DisplayEntityID" });
            DropIndex("DB2018.SaleEntities", new[] { "Orderform_OrderformID" });
            DropIndex("DB2018.SaleEntities", new[] { "UserID" });
            DropIndex("DB2018.Options", new[] { "GAttributeID" });
            DropIndex("DB2018.GoodEntities", new[] { "BrandID" });
            DropPrimaryKey("DB2018.Logistics");
            DropPrimaryKey("DB2018.Coupons");
            AlterColumn("DB2018.Logistics", "LogisticID", c => c.Decimal(nullable: false, precision: 10, scale: 0, identity: true));
            AlterColumn("DB2018.Coupons", "NeedVIP", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            DropColumn("DB2018.SaleEntities", "Orderform_OrderformID");
            DropColumn("DB2018.SaleEntities", "UserID");
            DropColumn("DB2018.SaleEntities", "InCart");
            DropColumn("DB2018.Comments", "UserCommentTime");
            DropColumn("DB2018.Comments", "LevelRank");
            DropColumn("DB2018.Comments", "Detail");
            DropColumn("DB2018.Comments", "DisplayEntityID");
            DropColumn("DB2018.GoodEntities", "BrandID");
            DropColumn("DB2018.Coupons", "CouponID");
            DropTable("DB2018.CouponsUsers");
            DropTable("DB2018.SaleEntityOptions");
            DropTable("DB2018.OptionGoodEntities");
            DropTable("DB2018.Brands");
            DropTable("DB2018.GAttributes");
            DropTable("DB2018.Options");
            AddPrimaryKey("DB2018.Logistics", "LogisticID");
            AddPrimaryKey("DB2018.Coupons", "ID");
            CreateIndex("DB2018.Comments", "SaleEntityID");
            CreateIndex("DB2018.CommentInfoes", "CommentID");
            CreateIndex("DB2018.AttributeOptions", "GoodAttributeID");
            CreateIndex("DB2018.Coupons", "User_UserID");
            AddForeignKey("DB2018.LogisticInfoes", "LogisticID", "DB2018.Logistics", "LogisticID", cascadeDelete: true);
            AddForeignKey("DB2018.Comments", "SaleEntityID", "DB2018.SaleEntities", "ID", cascadeDelete: true);
            AddForeignKey("DB2018.CommentInfoes", "CommentID", "DB2018.Comments", "CommentID");
            AddForeignKey("DB2018.AttributeOptions", "GoodAttributeID", "DB2018.GoodAttributes", "GoodAttributeID");
            AddForeignKey("DB2018.Coupons", "User_UserID", "DB2018.Users", "UserID");
        }
    }
}
