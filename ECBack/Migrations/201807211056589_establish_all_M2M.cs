namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class establish_all_M2M : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("DB2018.Coupons", "User_UserID", "DB2018.Users");
            DropForeignKey("DB2018.CommentInfoes", "CommentID", "DB2018.Comments");
            DropForeignKey("DB2018.Comments", "SaleEntityID", "DB2018.SaleEntities");
            DropForeignKey("DB2018.AttributeOptions", "GoodAttributeID", "DB2018.GoodAttributes");
            DropForeignKey("DB2018.LogisticInfoes", "LogisticID", "DB2018.Logistics");
            DropIndex("DB2018.Coupons", new[] { "User_UserID" });
            DropIndex("DB2018.CommentInfoes", new[] { "CommentID" });
            DropIndex("DB2018.Comments", new[] { "SaleEntityID" });
            DropPrimaryKey("DB2018.Coupons");
            DropPrimaryKey("DB2018.AttributeOptions");
            DropPrimaryKey("DB2018.Logistics");
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
                "DB2018.AttributeOptionGoodEntities",
                c => new
                    {
                        AttributeOption_AttributeOptionID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        GoodEntity_GoodEntityID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.AttributeOption_AttributeOptionID, t.GoodEntity_GoodEntityID })
                .ForeignKey("DB2018.AttributeOptions", t => t.AttributeOption_AttributeOptionID, cascadeDelete: true)
                .ForeignKey("DB2018.GoodEntities", t => t.GoodEntity_GoodEntityID, cascadeDelete: true)
                .Index(t => t.AttributeOption_AttributeOptionID)
                .Index(t => t.GoodEntity_GoodEntityID);
            
            CreateTable(
                "DB2018.SaleEntityAttributeOptions",
                c => new
                    {
                        SaleEntity_ID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        AttributeOption_AttributeOptionID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.SaleEntity_ID, t.AttributeOption_AttributeOptionID })
                .ForeignKey("DB2018.SaleEntities", t => t.SaleEntity_ID, cascadeDelete: true)
                .ForeignKey("DB2018.AttributeOptions", t => t.AttributeOption_AttributeOptionID, cascadeDelete: true)
                .Index(t => t.SaleEntity_ID)
                .Index(t => t.AttributeOption_AttributeOptionID);
            
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
            AddColumn("DB2018.AttributeOptions", "AttributeOptionID", c => c.Decimal(nullable: false, precision: 10, scale: 0, identity: true));
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
            AddPrimaryKey("DB2018.AttributeOptions", "AttributeOptionID");
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
            AddForeignKey("DB2018.AttributeOptions", "GoodAttributeID", "DB2018.GoodAttributes", "GoodAttributeID", cascadeDelete: true);
            AddForeignKey("DB2018.LogisticInfoes", "LogisticID", "DB2018.Logistics", "LogisticID", cascadeDelete: true);
            DropColumn("DB2018.Coupons", "ID");
            DropColumn("DB2018.Coupons", "User_UserID");
            DropColumn("DB2018.Comments", "SaleEntityID");
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
            
            AddColumn("DB2018.Comments", "SaleEntityID", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AddColumn("DB2018.Coupons", "User_UserID", c => c.Decimal(precision: 10, scale: 0));
            AddColumn("DB2018.Coupons", "ID", c => c.Decimal(nullable: false, precision: 10, scale: 0, identity: true));
            DropForeignKey("DB2018.LogisticInfoes", "LogisticID", "DB2018.Logistics");
            DropForeignKey("DB2018.AttributeOptions", "GoodAttributeID", "DB2018.GoodAttributes");
            DropForeignKey("DB2018.SaleEntities", "UserID", "DB2018.Carts");
            DropForeignKey("DB2018.SaleEntities", "Orderform_OrderformID", "DB2018.Orderforms");
            DropForeignKey("DB2018.Logistics", "LogisticID", "DB2018.Orderforms");
            DropForeignKey("DB2018.CouponsUsers", "User_UserID", "DB2018.Users");
            DropForeignKey("DB2018.CouponsUsers", "Coupons_CouponID", "DB2018.Coupons");
            DropForeignKey("DB2018.GoodEntities", "BrandID", "DB2018.Brands");
            DropForeignKey("DB2018.SaleEntities", "UserID", "DB2018.Users");
            DropForeignKey("DB2018.Comments", "DisplayEntityID", "DB2018.DisplayEntities");
            DropForeignKey("DB2018.SaleEntityAttributeOptions", "AttributeOption_AttributeOptionID", "DB2018.AttributeOptions");
            DropForeignKey("DB2018.SaleEntityAttributeOptions", "SaleEntity_ID", "DB2018.SaleEntities");
            DropForeignKey("DB2018.AttributeOptionGoodEntities", "GoodEntity_GoodEntityID", "DB2018.GoodEntities");
            DropForeignKey("DB2018.AttributeOptionGoodEntities", "AttributeOption_AttributeOptionID", "DB2018.AttributeOptions");
            DropIndex("DB2018.CouponsUsers", new[] { "User_UserID" });
            DropIndex("DB2018.CouponsUsers", new[] { "Coupons_CouponID" });
            DropIndex("DB2018.SaleEntityAttributeOptions", new[] { "AttributeOption_AttributeOptionID" });
            DropIndex("DB2018.SaleEntityAttributeOptions", new[] { "SaleEntity_ID" });
            DropIndex("DB2018.AttributeOptionGoodEntities", new[] { "GoodEntity_GoodEntityID" });
            DropIndex("DB2018.AttributeOptionGoodEntities", new[] { "AttributeOption_AttributeOptionID" });
            DropIndex("DB2018.Logistics", new[] { "LogisticID" });
            DropIndex("DB2018.Brands", new[] { "BrandName" });
            DropIndex("DB2018.Comments", new[] { "DisplayEntityID" });
            DropIndex("DB2018.SaleEntities", new[] { "Orderform_OrderformID" });
            DropIndex("DB2018.SaleEntities", new[] { "UserID" });
            DropIndex("DB2018.GoodEntities", new[] { "BrandID" });
            DropPrimaryKey("DB2018.Logistics");
            DropPrimaryKey("DB2018.AttributeOptions");
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
            DropColumn("DB2018.AttributeOptions", "AttributeOptionID");
            DropColumn("DB2018.GoodEntities", "BrandID");
            DropColumn("DB2018.Coupons", "CouponID");
            DropTable("DB2018.CouponsUsers");
            DropTable("DB2018.SaleEntityAttributeOptions");
            DropTable("DB2018.AttributeOptionGoodEntities");
            DropTable("DB2018.Brands");
            AddPrimaryKey("DB2018.Logistics", "LogisticID");
            AddPrimaryKey("DB2018.AttributeOptions", "GoodAttributeID");
            AddPrimaryKey("DB2018.Coupons", "ID");
            CreateIndex("DB2018.Comments", "SaleEntityID");
            CreateIndex("DB2018.CommentInfoes", "CommentID");
            CreateIndex("DB2018.Coupons", "User_UserID");
            AddForeignKey("DB2018.LogisticInfoes", "LogisticID", "DB2018.Logistics", "LogisticID", cascadeDelete: true);
            AddForeignKey("DB2018.AttributeOptions", "GoodAttributeID", "DB2018.GoodAttributes", "GoodAttributeID");
            AddForeignKey("DB2018.Comments", "SaleEntityID", "DB2018.SaleEntities", "ID", cascadeDelete: true);
            AddForeignKey("DB2018.CommentInfoes", "CommentID", "DB2018.Comments", "CommentID");
            AddForeignKey("DB2018.Coupons", "User_UserID", "DB2018.Users", "UserID");
        }
    }
}
