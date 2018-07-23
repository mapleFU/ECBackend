using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ECBack.Models
{
    public class CartRecord
    {
        [Key]
        public int CartRecordID { get; set; }

        

        [ForeignKey("UserID")]
        [JsonIgnore]
        public Cart Cart { get; set; }

        [Index]
        [ForeignKey("Cart")]
        public int UserID { get; set; }

        [Index]
        public int SaleEntityID { get; set; }

        [ForeignKey("SaleEntityID")]
        public SaleEntity SaleEntity { get; set; }

        [Required]
        public int RecordNum { get; set; }

        public CartRecord()
        {
            RecordNum = 1;
        }

    }

    public class Cart
    {
        [JsonIgnore]
        public User User { get; set; }

        [Required]
        [Key]
        [ForeignKey("User")]
        public int UserID { get; set; }

        public ICollection<CartRecord> CartRecords { get; set; }

        public async Task<decimal> TotalPrice(OracleDbContext dbContext)
        {
            await dbContext.Entry(this).Reference(u => u.CartRecords).LoadAsync();
            decimal price = 0;
            throw new NotImplementedException();
            foreach (var p in dbContext.SaleEntities)
            {
                price += p.Price;
            }
            return price;
        }

        
        //public async void AddToCart(OracleDbContext dbContext, SaleEntity entity)
        //{
        //    await dbContext.Entry(this).Reference(u => u.SaleEntities).LoadAsync();
        //    SaleEntities.Add(entity);
        //    await dbContext.SaveChangesAsync();
        //}
        
    }


}