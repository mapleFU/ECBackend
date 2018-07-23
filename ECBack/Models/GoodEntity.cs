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
        public GoodEntity()
        {
            GoodEntityState = 1;
            FavoriteNum = 0;
        }

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
        
        public string Detail { get; set; }

        public string DetailImages { get; set; }

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
        public int FavoriteNum { get; set; }

        //public virtual ICollection<Option> AttributeOptions { get; set; }

        public int BrandID { get; set; }

        [ForeignKey("BrandID")]
        public Brand Brand { get; set; }

        [JsonIgnore]
        public virtual ICollection<Category> Categories { get; set; }

        /// <summary>
        /// 商品属性
        /// </summary>
        public ICollection<GAttribute> GAttributes { get; set; }

        public ICollection<Image> Images { get; set; }

        
       
    }

    public class SaleEntity
    {
        [Key]
        [Required]
        public int SaleEntityID { get; set;}
        
        /// <summary>
        /// 购买/加入购物车的价格
        /// </summary>
        
        [Required]
        public decimal Price { get; set; }

        [Index]
        public int GoodEntityID { get; set; }

        [ForeignKey("GoodEntityID")]
        public GoodEntity GoodEntity;

        public virtual ICollection<Option> AttributeOptions { get; set; }

        [JsonIgnore]
        public ICollection<Comment> Comments { get; set; }
    }

    //public class DisplayEntity
    //{
    //    [Key]
    //    [Required]
    //    public int DisplayEntityID { get; set; }

    //    /// <summary>
    //    /// 商品的售价
    //    /// </summary>

    //    [Required]
    //    public decimal Price { get; set; }

    //    /// <summary>
    //    /// 展示的图片
    //    /// </summary>

    //    [Required]
    //    public ICollection<Image> Imgs { get; set; }

    //    /// <summary>
    //    /// id of foreign key good entity
    //    /// </summary>
    //    [Index]
    //    [Required]
    //    public int GoodEntityID { get; set; }

    //    [ForeignKey("GoodEntityID")]
    //    public GoodEntity GoodEntity { get; set; }

    //    public ICollection<Question> Questions { get; set; }

    //    public ICollection<Comment> Comments { get; set; }

    //}


    public class Brand
    {
        [Key]
        [Required]
        public int BrandID { get; set; }

        [Index]
        [MaxLength(50)]
        [Required]
        public string BrandName { get; set; }

        [MaxLength(500)]
        public string Detail { get; set; }


        public ICollection<GoodEntity> GoodEntities { get; set; }
    }
}