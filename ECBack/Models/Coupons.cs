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
        public Coupons()
        {
            NeedVIP = false;
        }


        /// <summary>
        /// Id of coupon
        /// </summary>
        [Key]
        public int CouponID { get; set; }

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
        public Boolean NeedVIP { get; set; }

        [JsonIgnore]
        public int CategoryID { get; set; }

        // 打折范围（类别）
        [ForeignKey("CategoryID")]
        public Category Category { get; set; }

        [JsonIgnore]
        public virtual ICollection<User>  Users { get; set; }

    }
}