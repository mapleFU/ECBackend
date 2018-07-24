namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatethetableofquestionandattrbutes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("DB2018.Questions", "SaleEntityID", "DB2018.SaleEntities");
            DropForeignKey("DB2018.Replies", "QuestionID", "DB2018.Questions");
            DropIndex("DB2018.Questions", new[] { "SaleEntityID" });
            DropIndex("DB2018.Replies", new[] { "QuestionID" });
            RenameColumn(table: "DB2018.Replies", name: "QuestionID", newName: "Question_QuestionID");
            AlterColumn("DB2018.Replies", "Question_QuestionID", c => c.Decimal(precision: 10, scale: 0));
            CreateIndex("DB2018.Replies", "Question_QuestionID");
            AddForeignKey("DB2018.Replies", "Question_QuestionID", "DB2018.Questions", "QuestionID");
            DropColumn("DB2018.Questions", "SaleEntityID");
        }
        
        public override void Down()
        {
            AddColumn("DB2018.Questions", "SaleEntityID", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            DropForeignKey("DB2018.Replies", "Question_QuestionID", "DB2018.Questions");
            DropIndex("DB2018.Replies", new[] { "Question_QuestionID" });
            AlterColumn("DB2018.Replies", "Question_QuestionID", c => c.Decimal(nullable: false, precision: 10, scale: 0));
            RenameColumn(table: "DB2018.Replies", name: "Question_QuestionID", newName: "QuestionID");
            CreateIndex("DB2018.Replies", "QuestionID");
            CreateIndex("DB2018.Questions", "SaleEntityID");
            AddForeignKey("DB2018.Replies", "QuestionID", "DB2018.Questions", "QuestionID", cascadeDelete: true);
            AddForeignKey("DB2018.Questions", "SaleEntityID", "DB2018.SaleEntities", "SaleEntityID", cascadeDelete: true);
        }
    }
}
