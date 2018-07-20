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
    /// 一个商品的所有评论
    /// </summary>
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }

        public ICollection<CommentInfo> CommentInfos { get; set; }

        [ForeignKey("SaleEntity")]
        public int SaleEntityID { get; set; }

        [JsonIgnore]
        public SaleEntity SaleEntity { get; set; }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class CommentInfo
    {
        [Key]
        public int ID { get; set; }

        [Index]
        public int CommentID { get; set; }

        [ForeignKey("CommentID")]
        [JsonIgnore]
        public Comment Comment { get; set; }

        [MaxLength(400)]
        public string Detail { get; set; }

        //评价等级 修改表, 0 没有评价， 1 差评 ，2 中等，3好评
        public int LevelRank { get; set; }

        //时间
        public DateTime UserCommentTime { get; set; }


        public CommentInfo()
        {
            UserCommentTime = DateTime.Now;
            LevelRank = 0;
        }
    }
}