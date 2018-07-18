using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECBack.Models
{
    public class GoodAttribute
    {
        /// <summary>
        /// ID of attribute
        /// </summary>
        [Key]
        [Required]
        public int GoodAttributeID { get; set; }

        [Required]
        [MaxLength(100)]
        public string GoodAttributeName { get; set;}

        public ICollection<AttributeOption> GoodAttributeOptions { get; set; }
    }

    public class AttributeOption
    {
        [Key]
        [Required]
        public int GoodAttributeID { get; set; }

        [Required]
        [ForeignKey("GoodAttributeID")]
        public GoodAttribute GoodAttribute { get; set; }

        /// <summary>
        /// 商品的描述
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Describe { get; set; }

    }

}