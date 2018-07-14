using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECBack.Models
{
    /// <summary>
    /// 收货地址
    /// </summary>
    public class Address
    {
        /// <summary>
        /// 地址ID
        /// </summary>
        [Key]
        public int AddressID { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        [MaxLength(50)]
        public string ReceiverName { get; set; }

        /// <summary>
        /// 收获手机号
        /// </summary>
        [MaxLength(50)]
        public string Phone { get; set; }

        /// <summary>
        /// 所在省份
        /// TODO:需要弄可选？
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 所在市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 街区
        /// </summary>
        public string Block { get; set; }

        public string DetailAddress { get; set; }

        /// <summary>
        /// 是否是默认地址
        /// </summary>
        public Boolean IsDefault { get; set; }

        /// <summary>
        /// User's Id
        /// </summary>
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}