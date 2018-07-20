using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ECBack.Models
{
    public class Coupons
    {
        /// <summary>
        /// Id of coupon
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 购买最低阈值
        /// </summary>
        [Required]
        public int Min { get; set; }

        /// <summary>
        /// 可减少金额
        /// </summary>
        [Required]
        public int Decrease { get; set; }

        /// <summary>
        /// 是否需要会员权限
        /// </summary>
        [Required]
        public int NeedVIP { get; set; }

        /// <summary>
        /// 打折范围（类别）
        /// </summary>
        [Required]
        public int CategoryID { get; set; }


        [ForeignKey("CategoryID")]
        public Category Category { get; set; }

    }
}