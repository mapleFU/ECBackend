using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECBack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECBack2.Controllers
{
    

    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly OracleDbContext _dbContext;

        public UsersController(OracleDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}