using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECBack.Models
{
    /// <summary>
    /// h货物商品的实体，用来存储实际的货物物品
    /// SPU in table
    /// 
    /// TODO: 加上文件 ：https://www.mikesdotnetting.com/article/259/asp-net-mvc-5-with-ef-6-working-with-files
    /// </summary>
    public class GoodEntity
    {
        /// <summary>
        /// Id of goo
        /// </summary>
        public int GoodEntityID { get; set; }
        
        /// <summary>
        /// Name OF ENTITY
        /// </summary>
        [MaxLength(100)]
        public string GoodName { get; set; }

        /// <summary>
        /// brief introduction of good
        /// </summary>
        [MaxLength(1000)]
        public string Brief { get; set; }
        
        /// <summary>
        /// detail introduction of good
        /// </summary>
        public string Detail { get; set; }
        
        /// <summary>
        /// 商品的库存
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// 寄出的省份
        /// </summary>
        public string SellProvince { get; set; }

        /// <summary>
        /// 失效/下架
        /// </summary>
        public int GoodEntityState { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public GoodEntity()
        {
            Categories = new HashSet<Category>();
        }
    }

    public class SaleEntity
    {
        public int ID { get; set;}
        
        /// <summary>
        /// 售价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// id of foreign key good entity
        /// </summary>
        public int GoodEntityID { get; set; }

        [ForeignKey("GoodEntityID")]
        public GoodEntity GoodEntity { get; set; }

        /// <summary>
        /// 这玩意的总量
        /// </summary>
        public int Amount { get; set;}


    }
}