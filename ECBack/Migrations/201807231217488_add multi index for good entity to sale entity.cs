namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmultiindexforgoodentitytosaleentity : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("DB2018.SaleEntities", "GoodEntityID", "DB2018.GoodEntities", "GoodEntityID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("DB2018.SaleEntities", "GoodEntityID", "DB2018.GoodEntities");
        }
    }
}
