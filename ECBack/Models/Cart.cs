using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECBack.Models
{
    public class Cart
    {
        [Required]
        [Key]
        public int UserID;


    }
}