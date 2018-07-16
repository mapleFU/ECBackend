using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECBack.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [MaxLength(50)]
        public string NickName { get; set; }

        [MaxLength(50)]
        public string RealName { get; set; }

        [Index]
        [Required]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        // 'f' for female, 'm' for male ,'n' for none, https://stackoverflow.com/questions/6760765/how-do-i-map-a-char-property-using-the-entity-framework-4-1-code-only-fluent-a
        [Column(TypeName = "char")]
        public string Gender { get; set; }

        // https://stackoverflow.com/questions/5658216/entity-framework-code-first-date-field-creation
        [Column(TypeName = "Date")]
        public DateTime BirthDay { get; set; }

        // 居住地
        [MaxLength(100)] public string Local { get; set; }
        // 家乡
        [MaxLength(100)] public string Home { get; set; }

        // password 的哈希值
        [Required]
        public string PasswordHash { get; set; }

        public List<Address> Addresses { get; set; }

        public List<Orderform> Orderforms { get; set; }
    }
}