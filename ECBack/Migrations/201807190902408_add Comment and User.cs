namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCommentandUser : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("DB2018.Comments", t => t.CommentID)
                .Index(t => t.CommentID);
            
            CreateTable(
                "DB2018.Comments",
                c => new
                    {
                        CommentID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        SaleEntityID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("DB2018.SaleEntities", t => t.SaleEntityID, cascadeDelete: true)
                .Index(t => t.SaleEntityID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("DB2018.Comments", "SaleEntityID", "DB2018.SaleEntities");
            DropForeignKey("DB2018.CommentInfoes", "CommentID", "DB2018.Comments");
            DropIndex("DB2018.Comments", new[] { "SaleEntityID" });
            DropIndex("DB2018.CommentInfoes", new[] { "CommentID" });
            DropTable("DB2018.Comments");
            DropTable("DB2018.CommentInfoes");
        }
    }
}
