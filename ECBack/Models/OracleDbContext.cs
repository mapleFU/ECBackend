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

        public OracleDbContext(string connectString) : base("DATA SOURCE= (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 10.60.42.201)(PORT = 1521))  (CONNECT_DATA = (SERVICE_NAME = xe) ) );PASSWORD=db2018;PERSIST SECURITY INFO=True;USER ID=db2018")
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
            //    .WithMany(usr => usr.orderforms)
            //    .HasForeignKey(ad => ad.FormCustomer)
            //    .WillCascadeOnDelete(true);

            
        }

        public System.Data.Entity.DbSet<ECBack.Models.Orderform> Orderforms { get; set; }

        public System.Data.Entity.DbSet<ECBack.Models.Address> Addresses { get; set; }
    }
}
