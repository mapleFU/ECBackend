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
    /// 收藏，联系用户与SPU
    /// https://stackoverflow.com/questions/7050404/create-code-first-many-to-many-with-additional-fields-in-association-table
    /// </summary>
    public class Favorite
    {

        [Key]
        public int FavoriteID { get; set; }

        /// <summary>
        /// 便于对整个商品的内容分组运算
        /// </summary>
        [Index]
        public int GoodEntityID { get; set; }

        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("GoodEntityID")]
        public virtual GoodEntity GoodEntity { get; set; }

        [NotMapped]
        public int FavorUser
        {
            get
            {
                throw new NotImplementedException();
                // return 0;
            }
        }
    }
}