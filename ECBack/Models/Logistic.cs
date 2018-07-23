using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace ECBack.Models
{
    /// <summary>
    /// 一次物流的信息
    /// </summary>
    public class LogisticInfo
    {
        [Key]
        public int LogisticInfoId { get; set; }

        //[Required]
        //public int OrderformID { get; set; }

        //[JsonIgnore]
        //public Orderform Orderform { get; set; }
        /// <summary>
        /// Foreign key
        /// </summary>
        [JsonIgnore]
        [Index]
        [Required]
        public int LogisticID { get; set; }

        public DateTime Time { get; set; }

        public string Position { get; set; }

        /// <summary>
        /// 表示离开和到达的状态
        /// </summary>
        public int State { get; set; }

        public LogisticInfo()
        {
            this.Time = DateTime.Now;
        }
    }
    /// <summary>
    /// 物流总览
    /// </summary>
    public class Logistic
    {
        [Required]
        [Key]
        [ForeignKey("Orderform")]
        public int LogisticID { get; set; }

        /// <summary>
        /// m描述属性的变量
        /// TODO: 清晰的描述，并配置影子属性
        /// </summary>
        public int State { get; set; }

        public string FromAddress { get; set; }
        public string ToAddress { get; set; }

        public List<LogisticInfo> LogisticInfos { get; set; }
 
        public virtual Orderform Orderform { get; set; }

    }
}