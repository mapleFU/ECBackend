using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECBack.Models
{
    public class Cart
    {
        public User User;

        [Required]
        [Key]
        [ForeignKey("User")]
        public int UserID;


    }
}