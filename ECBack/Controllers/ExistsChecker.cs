using ECBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ECBack.Controllers
{
    public class ExistsChecker
    {
        private OracleDbContext _db;

        public ExistsChecker()
        {
            _db = new OracleDbContext();
        }

        public async Task<User> CheckUser(int UserID)
        {

            return await _db.Users.FindAsync(UserID);
           
        }
    }
}