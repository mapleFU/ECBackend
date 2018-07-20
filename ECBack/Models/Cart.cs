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
    public class Cart
    {
        [JsonIgnore]
        public User User { get; set; }

        [Required]
        [Key]
        [ForeignKey("User")]
        public int UserID { get; set; }

        
        ICollection<SaleEntity> SaleEntities { get; set; }

        public async Task<decimal> TotalPrice(OracleDbContext dbContext)
        {
            await dbContext.Entry(this).Reference(u => u.SaleEntities).LoadAsync();
            decimal price = 0;
            foreach (var p in dbContext.SaleEntities)
            {
                price += p.Price;
            }
            return price;
        }

        
        public async void AddToCart(OracleDbContext dbContext, SaleEntity entity)
        {
            await dbContext.Entry(this).Reference(u => u.SaleEntities).LoadAsync();
            SaleEntities.Add(entity);
            await dbContext.SaveChangesAsync();
        }
        
    }
}