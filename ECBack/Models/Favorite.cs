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
    /// </summary>
    public class Favorite
    {
        [Key]
        public int FavoriteID { get; set; }

        public int NumOfUser { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
        [ForeignKey("GoodEntityID")]
        public virtual GoodEntity GoodEntity { get; set; }
    }
}