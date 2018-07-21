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
    /// 评论
    /// </summary>
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }

        //[ForeignKey("DisplayEntity")]
        //public int DisplayEntityID { get; set; }

        //[JsonIgnore]
        //public DisplayEntity DisplayEntity { get; set; }

        [MaxLength(400)]
        public string Detail { get; set; }

        //评价等级 修改表, 0 没有评价， 1 差评 ，2 中等，3好评
        public int LevelRank { get; set; }

        //时间
        public DateTime UserCommentTime { get; set; }


        public Comment()
        {
            UserCommentTime = DateTime.Now;
            LevelRank = 0;
        }
    }
}