using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECBack.Models
{
    public class Orderform
    {
        [Key]
        public int OrderformID { get; set; }

        // 交易时间
        public DateTime TransacDateTime { get; set; }

        // TODO: 是否使用shadow状态属性
        // 订单状态： 已完成／已发货／待支付
        public int State { get; set; }

        // 总金额
        public float TotalPrice { get; set; }

        public int UserID { get; set; }

        /// <summary>
        /// 非实体属性
        /// </summary>
        [ForeignKey("UserID")]
        public User FormCustomer { get; set; }

    }
}