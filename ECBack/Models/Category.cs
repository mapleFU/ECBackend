using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECBack.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        /// <summary>
        /// Name of the category
        /// </summary>
        [MaxLength(50)]
        [Index]
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<GoodEntity> GoodEntities { get; set; }
    }

}