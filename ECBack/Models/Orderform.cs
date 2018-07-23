using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECBack.Models
{
    /// <summary>
    /// 描述订单的类
    /// 
    /// </summary>
    public class Orderform
    {
        [Key]
        public int OrderformID { get; set; }

        // 交易时间
        [Required]
        public DateTime TransacDateTime { get; set; }

        // TODO: 是否使用shadow状态属性
        // 订单状态： 已完成／已发货／待支付
        [Required]
        [Range(0, 3)]
        public int State { get; set; }

        // 总金额
        [Required]
        public float TotalPrice { get; set; }

        // 多个 SaleEntities
        ICollection<SaleEntity> SaleEntities { get; set; }

        [Required]
        public int UserID { get; set; }

        /// <summary>
        /// 非实体属性
        /// </summary>
        [ForeignKey("UserID")]
        [JsonIgnore]
        public User FormCustomer { get; set; }

        public Orderform()
        {
            TransacDateTime = DateTime.Now;
            // 订单成为默认状态。
            State = 2;
        }

        
        public virtual Logistic Logistic { get; set; }

        public int AddressID { get; set; }

        [ForeignKey("AddressID")]
        public Address Address { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string Status
        {
            get
            {
                if (State == 0)
                {
                    return "已完成";
                } else if (State == 1)
                {
                    return "已发货";
                } else if (State == 2)
                {
                    return "待支付";
                } else
                {
                    throw new ArgumentException("Value of state error!");
                }
            }
        }
    }
}