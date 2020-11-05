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

        // PUT: api/Assets/5
        /// <summary>
        /// Καλείται όταν κάνουμε edit ένα asset. Αν δεν θέλουμε να έχει τοποθεσία, τότε location = null.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public Asset PutAsset(int id, Asset asset)
        {
            var editAsset = _context.Assets.Include(x => x.Location).SingleOrDefault(x => x.Id == id);

            if (editAsset == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            if (!asset.IsValid())
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            //ελέγχουμε αν το asset ανήκει σε αυτήν την εταιρία
            var company = _context.Companies.Include(x => x.Assets).SingleOrDefault(x => x.Id == editAsset.CompanyId);

            if (company == null || !company.Assets.Any(x => x.Id == id))
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }


            var location = asset.Location;

            editAsset.Name = asset.Name;
            editAsset.Description = asset.Description ?? "";

            editAsset.CreatedAt = DateTime.UtcNow;
            editAsset.Location = location;

            var editLocation = editAsset.Location;

            //περίπτωση 1: είχε ήδη location και εξακολουθεί να έχει
            if (editLocation != null && location != null)
            {
                editLocation.X = location.X;
                editLocation.Y = location.Y;
                editLocation.Country = location.Country;
                editLocation.City = location.City;
                editLocation.Street = location.Street;
                editLocation.Number = location.Number;
                editLocation.ZipCode = location.ZipCode;
            }

            //περίπτωση 2: είχε ήδη location ενώ τώρα δεν έχει
            if (editLocation != null && location == null)
            {
                editAsset.LocationId = null;

                _context.Locations.Remove(editLocation);
                _context.SaveChanges();
            }

            //περίπτωση 3: δεν είχε location ενώ τώρα έχει
            if (editLocation == null && location != null)
            {
                editLocation = new Location();

                editLocation.X = location.X;
                editLocation.Y = location.Y;
                editLocation.Country = location.Country;
                editLocation.City = location.City;
                editLocation.Street = location.Street;
                editLocation.Number = location.Number;
                editLocation.ZipCode = location.ZipCode;

                _context.Locations.Add(editLocation);
                _context.SaveChanges();

                editAsset.LocationId = editLocation.Id;
            }

            //περίπτωση 4: δεν είχε location και ούτε τώρα έχει
            //δεν χρειάζεται να γίνει κάτι

            _context.SaveChanges();


            return editAsset;
        }

        // POST: api/Assets
        /// <summary>
        /// Καλείται όταν προσθέτουμε ένα asset. Αν δεν θέλουμε να έχει τοποθεσία, τότε location = null.
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        [HttpPost]
        public Asset PostAsset(Asset asset)
        {

            var location = asset.Location;

            if (location != null)
            {
                _context.Locations.Add(location);
                _context.SaveChanges();
            }
            

            asset.Description = asset.Description ?? "";

            if (location == null)
            {
                asset.LocationId = null;
            } else
            {
                asset.LocationId = location.Id;
            }

            _context.Assets.Add(asset);
            _context.SaveChanges();

            return asset;
        }

        // DELETE: api/Assets/5
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
