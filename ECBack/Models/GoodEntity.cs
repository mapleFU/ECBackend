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
        [Key]
        public int GoodEntityID { get; set; }
        
        /// <summary>
        /// Name OF ENTITY
        /// </summary>
        [MaxLength(100)]
        [Required]
        [Index]
        public string GoodName { get; set; }

        /// <summary>
        /// brief introduction of good
        /// </summary>
        [MaxLength(1000)]
        [Required]
        public string Brief { get; set; }
        
        /// <summary>
        /// detail introduction of good
        /// </summary>
        //  public string Detail { get; set; }
        
        /// <summary>
        /// 商品的库存
        /// </summary>
        [Required]
        public int Stock { get; set; }

        /// <summary>
        /// 寄出的省份
        /// </summary>
        [Required]
        public string SellProvince { get; set; }

        /// <summary>
        /// 失效/下架
        /// </summary>
        [Required]
        public int GoodEntityState { get; set; }

        /// <summary>
        /// 收藏人数
        /// </summary>
        [Required]
        public string FavoriteNum { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        
    }

    public class SaleEntity
    {
        [Key]
        [Required]
        public int ID { get; set;}
        
        /// <summary>
        /// 购买/加入购物车的价格
        /// </summary>
        
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// id of foreign key DisplayEntity
        /// </summary>
        [Index]
        [Required]
        public int DisplayEntityID { get; set; }

        [ForeignKey("DisplayEntityID")]
        public DisplayEntity DisplayEntity { get; set; }

        /// <summary>
        /// 购买/加入购物车的数量
        /// </summary>
        [Required]
        public int Amount { get; set;}


    }

    public class DisplayEntity
    {
        [Key]
        [Required]
        public int ID { get; set; }

        /// <summary>
        /// 商品的售价
        /// </summary>

        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// 展示的图片
        /// </summary>

        [Required]
        public ICollection<DisplayImg> Imgs { get; set; }

        /// <summary>
        /// id of foreign key good entity
        /// </summary>
        [Index]
        [Required]
        public int GoodEntityID { get; set; }

        [ForeignKey("GoodEntityID")]
        public GoodEntity GoodEntity { get; set; }



    }


    public class DisplayImg
    {
        /// <summary>
        /// Id of Imgs
        /// </summary>
        [Key] 
        public int ID { get; set; }

        [MaxLength(1000)]
        [Required]
        public string LargeImg { get; set; }

        [MaxLength(1000)]
        [Required]
        public string MediumImg { get; set; }

        [MaxLength(1000)]
        [Required]
        public string SmallImg { get; set; }

        /// <summary>
        /// 是否为主图
        /// </summary>
        [Required]
        public Boolean IsMain { get; set; }
    
        /// <summary>
        /// id of foreign key DisplayEntity
        /// </summary>
        [Index]
        [Required]
        public int DisplayEntityID { get; set; }

        [ForeignKey("DisplayEntityID")]
        public DisplayEntity DisplayEntity { get; set; }
    }
}