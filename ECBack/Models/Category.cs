using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECBack.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        /// <summary>
        /// Name of the category
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }
    }

}