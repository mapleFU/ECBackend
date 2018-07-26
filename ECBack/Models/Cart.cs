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
    public class GoodEntityCartSchema
    {
        public string GoodName { get; set; }
        public int Stock { get; set; }
        [JsonIgnore]
        public string DetailImages { get; set; }

        public string ImageURL
        {
            get
            {
                if (DetailImages == null)
                {
                    return null;
                } else
                {
                    return DetailImages.Split(',')[0];
                }
            }
        }
        public int? GoodEntityState { get; set; }
        public int GoodEntityID { get; set;}
    }

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
        public virtual SaleEntity SaleEntity { get; set; }

        [Required]
        public int RecordNum { get; set; }

        public CartRecord()
        {
            RecordNum = 1;
        }

        private OracleDbContext _db = new OracleDbContext();

        // TODO: can we make it more clearly?
        [NotMapped]
        public GoodEntityCartSchema RelatedGoodEntity
        {
            get
            {
                if (SaleEntity == null)
                {
                    _db.Entry(this).Reference(cd => cd.SaleEntity).Load();
                }
                int geID = this.SaleEntity.GoodEntityID;
                return _db.GoodEntities.Select(ge => new GoodEntityCartSchema() {
                    GoodName = ge.GoodName,
                    GoodEntityState = ge.GoodEntityState,
                    DetailImages = ge.DetailImages,
                    Stock = ge.Stock,
                    GoodEntityID = ge.GoodEntityID
                }).First(e => e.GoodEntityID == geID);
            }
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

        public virtual ICollection<CartRecord> CartRecords { get; set; }

        public async Task<decimal> TotalPrice(OracleDbContext dbContext)
        {
            await dbContext.Entry(this).Collection(u => u.CartRecords).LoadAsync();
            decimal price = 0;
            
            foreach (var p in this.CartRecords)
            {
                await dbContext.Entry(p).Reference(pp => pp.SaleEntity).LoadAsync();
                price += p.SaleEntity.Price * p.RecordNum;
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