namespace ECBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initializemodels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DB2018.Addresses",
                c => new
                    {
                        AddressID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        ReceiverName = c.String(maxLength: 50),
                        Phone = c.String(maxLength: 50),
                        Province = c.String(),
                        City = c.String(),
                        Block = c.String(),
                        DetailAddress = c.String(),
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
                        NickName = c.String(maxLength: 50),
                        RealName = c.String(maxLength: 50),
                        PhoneNumber = c.String(maxLength: 11),
                        Gender = c.String(maxLength: 2000, fixedLength: true, unicode: false),
                        BirthDay = c.DateTime(nullable: false),
                        Local = c.String(maxLength: 100),
                        Home = c.String(maxLength: 100),
                        PasswordHash = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("DB2018.Orderforms", "UserID", "DB2018.Users");
            DropForeignKey("DB2018.Addresses", "UserID", "DB2018.Users");
            DropIndex("DB2018.Orderforms", new[] { "UserID" });
            DropIndex("DB2018.Addresses", new[] { "UserID" });
            DropTable("DB2018.Orderforms");
            DropTable("DB2018.Users");
            DropTable("DB2018.Addresses");
        }
    }
}
