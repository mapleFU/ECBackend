using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ECBack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class CategoryDTO
    {
        public string Name { get; set;}
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly OracleDbContext _context;

        public CategoriesController(OracleDbContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<List<Category>>> Get()
        {
            var categories = await (from b in _context.Categories
                                    orderby b.CategoryID
                                    select b).ToListAsync();
            return categories;
        }
    }
}