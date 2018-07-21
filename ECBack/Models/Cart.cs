using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECBack.Models
{
    public class Cart
    {
        public User User { get; set; }

        [Required]
        [Key]
        [ForeignKey("User")]
        public int UserID { get; set; }

        public ICollection<SaleEntity> SaleEntities { get; set; }

        [NotMapped]
        public int TotalPrice
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        void AddToCart()
        {
            throw new NotImplementedException();
        }
        
    }
}