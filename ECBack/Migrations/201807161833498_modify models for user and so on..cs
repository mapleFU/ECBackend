namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifymodelsforuserandsoon : DbMigration
    {
        public override void Up()
        {
            AlterColumn("DB2018.Users", "PhoneNumber", c => c.String(nullable: false, maxLength: 11));
            AlterColumn("DB2018.Users", "PasswordHash", c => c.String(nullable: false));
            CreateIndex("DB2018.Users", "PhoneNumber");
        }
        
        public override void Down()
        {
            DropIndex("DB2018.Users", new[] { "PhoneNumber" });
            AlterColumn("DB2018.Users", "PasswordHash", c => c.String());
            AlterColumn("DB2018.Users", "PhoneNumber", c => c.String(maxLength: 11));
        }
    }
}
