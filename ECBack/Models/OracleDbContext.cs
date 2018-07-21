using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ECBack.Models
{
    public class OracleDbContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public OracleDbContext() : base("name=OracleDbContext")
        {
        }

        public OracleDbContext(string connectString) : base("DATA SOURCE= (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 10.60.42.201)(PORT = 1521))  (CONNECT_DATA = (SERVICE_NAME = xe) ) );PASSWORD=DB2018;PERSIST SECURITY INFO=True;USER ID=DB2018")
        {
        }

        public System.Data.Entity.DbSet<ECBack.Models.User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("DB2018");

            // Address 属性
            //modelBuilder.Entity<Address>()
            //    .HasRequired(add => add.User)
            //    .WithMany(usr => usr.Addresses)
            //    .HasForeignKey(add => add.UserID)
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Orderform>()
            //    .HasRequired(ofm => ofm.FormCustomer)
            //    .WithMany(user => user.Orderforms)
            //    .HasForeignKey(ad => ad.OrderformID)
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<LogisticInfo>()
            //    .HasRequired(ofm => ofm.FormCustomer)
            //    .WithMany(user => user.Orderforms)
            //    .HasForeignKey(ad => ad.OrderformID)
            //    .WillCascadeOnDelete(true);



        }

        public System.Data.Entity.DbSet<ECBack.Models.Orderform> Orderforms { get; set; }

        public System.Data.Entity.DbSet<ECBack.Models.Address> Addresses { get; set; }

        public System.Data.Entity.DbSet<ECBack.Models.Logistic> Logistics { get; set; }

        public System.Data.Entity.DbSet<ECBack.Models.LogisticInfo> LogisticInfoes { get; set; }

        public System.Data.Entity.DbSet<ECBack.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<ECBack.Models.GoodEntity> GoodEntities { get; set; }

        public System.Data.Entity.DbSet<ECBack.Models.SaleEntity> SaleEntities { get; set; }

        public System.Data.Entity.DbSet<ECBack.Models.AttributeOption> AttributeOptions { get; set; }

        public System.Data.Entity.DbSet<ECBack.Models.GoodAttribute> GoodAttributes { get; set; }

        public System.Data.Entity.DbSet<ECBack.Models.Favorite> Favorites { get; set; }

        public System.Data.Entity.DbSet<ECBack.Models.Cart> Carts { get; set; }

        public System.Data.Entity.DbSet<ECBack.Models.Comment> Comments { get; set; }

        public System.Data.Entity.DbSet<ECBack.Models.CommentInfo> CommentInfoes { get; set; }

        public System.Data.Entity.DbSet<ECBack.Models.Coupons> Coupons { get; set; }

        public System.Data.Entity.DbSet<ECBack.Models.Reply> Replies { get; set; }

        public System.Data.Entity.DbSet<ECBack.Models.Question> Questions { get; set; }
    }
}
