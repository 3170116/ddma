using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ddma.Models;

namespace ddma.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AssetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Assets/5
        /// <summary>
        /// Επιστρέφει τις πληροφορίες ενός συγκεκριμένου asset.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Asset GetAsset(int id)
        {
            var asset = _context.Assets.SingleOrDefault(x => x.Id == id);

            if (asset == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            return asset;
        }

        [HttpDelete("{id}")]
        public int DeleteAsset(int id)
        {
            var asset = _context.Assets.SingleOrDefault(x => x.Id == id);

            if (asset == null)
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            _context.Assets.Remove(asset);
            _context.SaveChanges();

            return id;
        }

    }
}
