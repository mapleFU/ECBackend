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
    public class VIP
    {
        private readonly int[] LevelRequired = new int[] { 100, 500, 1500, 5000, 10000, 30000  };

        [JsonIgnore]
        [ForeignKey("UserID")]
        public User User;

        [Key]
        public int UserID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public int TotalCredits { get; set; }

        public int AvailCredits { get; set; }


        [NotMapped]
        public int Level
        {
            get
            {
                int cnt = 0;
                foreach (int v in LevelRequired)
                {
                    if (TotalCredits > v) ++cnt;
                    else break;
                }
                return cnt;
            }
        }

        public VIP()
        {
            StartDate = DateTime.Now;
            TotalCredits = AvailCredits = 0;
        }

        public static VIP CreateVIP (TimeSpan duration)
        {
            var vip = new VIP();
            vip.DueDate = vip.StartDate.Add(duration);
            return vip;
        }
        
        public bool HasEnoughAuth(int requiredLevel)
        {
            return Level >= requiredLevel;
        }
    }
}