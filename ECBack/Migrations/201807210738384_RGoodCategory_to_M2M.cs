namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RGoodCategory_to_M2M : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("DB2018.GoodEntities", "Category_CategoryID", "DB2018.Categories");
            DropForeignKey("DB2018.SaleEntities", "GoodEntityID", "DB2018.GoodEntities");
            DropIndex("DB2018.GoodEntities", new[] { "Category_CategoryID" });
            DropIndex("DB2018.SaleEntities", new[] { "GoodEntityID" });
            CreateTable(
                "DB2018.Coupons",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        Min = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Decrease = c.Decimal(nullable: false, precision: 10, scale: 0),
                        NeedVIP = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CategoryID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        User_UserID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("DB2018.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("DB2018.Users", t => t.User_UserID)
                .Index(t => t.CategoryID)
                .Index(t => t.User_UserID);
            
            CreateTable(
                "DB2018.DisplayEntities",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GoodEntityID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("DB2018.GoodEntities", t => t.GoodEntityID, cascadeDelete: true)
                .Index(t => t.GoodEntityID);
            
            CreateTable(
                "DB2018.DisplayImgs",
                c => new
                    {
                        ID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        LargeImg = c.String(nullable: false, maxLength: 1000),
                        MediumImg = c.String(nullable: false, maxLength: 1000),
                        SmallImg = c.String(nullable: false, maxLength: 1000),
                        IsMain = c.Decimal(nullable: false, precision: 1, scale: 0),
                        DisplayEntityID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("DB2018.DisplayEntities", t => t.DisplayEntityID, cascadeDelete: true)
                .Index(t => t.DisplayEntityID);
            
            CreateTable(
                "DB2018.Questions",
                c => new
                    {
                        QuestionID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        Detail = c.String(maxLength: 50),
                        DisplayEntity_ID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.QuestionID)
                .ForeignKey("DB2018.DisplayEntities", t => t.DisplayEntity_ID)
                .Index(t => t.DisplayEntity_ID);
            
            CreateTable(
                "DB2018.Replies",
                c => new
                    {
                        ReplyID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        ReplyDetail = c.String(maxLength: 200),
                        UserCommentTime = c.DateTime(nullable: false),
                        QuestionID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.ReplyID)
                .ForeignKey("DB2018.Questions", t => t.QuestionID, cascadeDelete: true)
                .Index(t => t.QuestionID);
            
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
            
            AddColumn("DB2018.GoodEntities", "FavoriteNum", c => c.String(nullable: false));
            AddColumn("DB2018.SaleEntities", "DisplayEntityID", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            CreateIndex("DB2018.SaleEntities", "DisplayEntityID");
            AddForeignKey("DB2018.SaleEntities", "DisplayEntityID", "DB2018.DisplayEntities", "ID", cascadeDelete: true);
            DropColumn("DB2018.GoodEntities", "Detail");
            DropColumn("DB2018.GoodEntities", "Category_CategoryID");
            DropColumn("DB2018.SaleEntities", "GoodEntityID");
        }
        
        public override void Down()
        {
            AddColumn("DB2018.SaleEntities", "GoodEntityID", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            AddColumn("DB2018.GoodEntities", "Category_CategoryID", c => c.Decimal(precision: 10, scale: 0));
            AddColumn("DB2018.GoodEntities", "Detail", c => c.String());
            DropForeignKey("DB2018.SaleEntities", "DisplayEntityID", "DB2018.DisplayEntities");
            DropForeignKey("DB2018.Replies", "QuestionID", "DB2018.Questions");
            DropForeignKey("DB2018.Questions", "DisplayEntity_ID", "DB2018.DisplayEntities");
            DropForeignKey("DB2018.DisplayImgs", "DisplayEntityID", "DB2018.DisplayEntities");
            DropForeignKey("DB2018.DisplayEntities", "GoodEntityID", "DB2018.GoodEntities");
            DropForeignKey("DB2018.Coupons", "User_UserID", "DB2018.Users");
            DropForeignKey("DB2018.Coupons", "CategoryID", "DB2018.Categories");
            DropForeignKey("DB2018.GoodEntityCategories", "Category_CategoryID", "DB2018.Categories");
            DropForeignKey("DB2018.GoodEntityCategories", "GoodEntity_GoodEntityID", "DB2018.GoodEntities");
            DropIndex("DB2018.GoodEntityCategories", new[] { "Category_CategoryID" });
            DropIndex("DB2018.GoodEntityCategories", new[] { "GoodEntity_GoodEntityID" });
            DropIndex("DB2018.Replies", new[] { "QuestionID" });
            DropIndex("DB2018.Questions", new[] { "DisplayEntity_ID" });
            DropIndex("DB2018.DisplayImgs", new[] { "DisplayEntityID" });
            DropIndex("DB2018.DisplayEntities", new[] { "GoodEntityID" });
            DropIndex("DB2018.SaleEntities", new[] { "DisplayEntityID" });
            DropIndex("DB2018.Coupons", new[] { "User_UserID" });
            DropIndex("DB2018.Coupons", new[] { "CategoryID" });
            DropColumn("DB2018.SaleEntities", "DisplayEntityID");
            DropColumn("DB2018.GoodEntities", "FavoriteNum");
            DropTable("DB2018.GoodEntityCategories");
            DropTable("DB2018.Replies");
            DropTable("DB2018.Questions");
            DropTable("DB2018.DisplayImgs");
            DropTable("DB2018.DisplayEntities");
            DropTable("DB2018.Coupons");
            CreateIndex("DB2018.SaleEntities", "GoodEntityID");
            CreateIndex("DB2018.GoodEntities", "Category_CategoryID");
            AddForeignKey("DB2018.SaleEntities", "GoodEntityID", "DB2018.GoodEntities", "GoodEntityID", cascadeDelete: true);
            AddForeignKey("DB2018.GoodEntities", "Category_CategoryID", "DB2018.Categories", "CategoryID");
        }
    }
}
