using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECBack.Models
{
    public class GAttribute
    {
        /// <summary>
        /// ID of GAttribute
        /// </summary>
        [Key]
        [Required]
        public int GAttributeID { get; set; }

        [Required]
        [MaxLength(100)]
        public string GAttributeName { get; set;}

        public ICollection<Option> Options { get; set; }
    }

    public class Option
    {
        public Option()
        {
            this.GoodEntities = new HashSet<GoodEntity>();
            this.SaleEntities = new HashSet<SaleEntity>();
        }

        [Key]
        [Required]
        public int OptionID { get; set; }

        [Required]
        public int GAttributeID { get; set; }

        [Required]
        [ForeignKey("GAttributeID")]
        public GAttribute GAttribute { get; set; }

        /// <summary>
        /// 商品的描述
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Describe { get; set; }

        public ICollection<GoodEntity> GoodEntities { get; set; }
        public ICollection<SaleEntity> SaleEntities { get; set; }

    }

}