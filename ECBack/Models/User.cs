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
    interface ICustomPrincipal : IPrincipal
    {
        string PhoneNumber { get; set; }
        
        string PasswordHash { get; set; }
    }

    public class UserBasicInfo
    {
        public int? UserID { get; set; }
        public virtual string RealName { get; set; }
        public string NickName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDay { get; set; }
        public string Local { get; set; }
        public string Home { get; set; }
        public string PasswordHash { get; set; }
        public string Password { get; set; }

        public void Cast(User usr)
        {
            if (UserID == null)
            {
                return;
            }
            if (RealName != null)
            {
                usr.RealName = RealName;
            } 
            if (NickName != null)
            {
                usr.NickName = NickName;
            }
            if (PhoneNumber != null)
            {
                usr.PhoneNumber = PhoneNumber;
            }
            if (BirthDay != null)
            {
                usr.BirthDay = BirthDay;
            }
        }

    }

    /// <summary>
    /// inspired by https://stackoverflow.com/questions/33170771/web-api-return-some-fields-from-model
    /// ignore json https://stackoverflow.com/questions/11851207/prevent-property-from-being-serialized-in-web-api
    /// </summary>
    public class User: ICustomPrincipal
    {
        [Key]
        public int UserID { get; set; }

        [MaxLength(50)]
        [Required]
        [Index]
        public string NickName { get; set; }

        [MaxLength(50)]
        public string RealName { get; set; }

        [Index]
        [Required]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        // 'f' for female, 'm' for male ,'n' for none, https://stackoverflow.com/questions/6760765/how-do-i-map-a-char-property-using-the-entity-framework-4-1-code-only-fluent-a
        [MaxLength(2)]
        public string Gender { get; set; }

        // https://stackoverflow.com/questions/5658216/entity-framework-code-first-date-field-creation
        [Column(TypeName = "Date")]
        [JsonIgnore]
        public DateTime BirthDay { get; set; }

        [NotMapped]
        public string BirthDayDate
        {
            get
            {
                return BirthDay.Date.ToString("dd/MM/yyyy");
            }
        }

        public User()
        {
            // 性别先不填
            Gender = "n";
            
        }

        // 居住地
        [MaxLength(100)] public string Local { get; set; }
        // 家乡
        [MaxLength(100)] public string Home { get; set; }

        // password 的哈希值
        [Required]
        [JsonIgnore]
        public string PasswordHash { get; set; }

        [JsonIgnore]
        public List<Address> Addresses { get; set; }

        [JsonIgnore]
        public List<Orderform> Orderforms { get; set; }

        [JsonIgnore]
        public ICollection<Favorite> Favorites { get; set; }

        [JsonIgnore]
        public virtual ICollection<Coupons> Coupons { get; set; }

        [NotMapped]
        [JsonIgnore]
        public IIdentity Identity { get; private set; }

        [JsonIgnore]
        public Cart Cart { get; set; }

        
        public VIP VIP { get; set; }

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}