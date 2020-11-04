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
    public class CompaniesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Companies
        [HttpGet]
        public IEnumerable<Company> GetCompanies()
        {
            return _context.Companies.Include("Users").Include("TaskAssignmentGroups").ToList();
        }

        // PUT: /Companies/{id}
        [HttpGet("{id}")]
        public Company GetCompany(int id)
        {
            var company = _context.Companies.Include(x => x.Users).SingleOrDefault(x => x.Id == id);

            if (company == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            return company;
        }

        // PUT: /Companies/{id}
        [HttpPut("{id}")]
        public Company PutCompany(int id, Company company)
        {
            var editCompany = _context.Companies.SingleOrDefault(x => x.Id == id);

            if (editCompany == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            if (!company.IsValid())
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            editCompany.Name = company.Name;
            editCompany.TimeZone = company.TimeZone;

            _context.SaveChanges();

            return company;
        }

        // POST: /Companies
        //Θα καλείται όταν ο super-supervisor κάνει εγγραφεί στην εφαρμογή.
        [HttpPost]
        public Company PostCompany(Company company)
        {

            if (!company.IsValid())
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            _context.Companies.Add(company);
            _context.SaveChanges();

            return company;
        }

        // DELETE: /Companies/{id}
        [HttpDelete("{id}")]
        public int DeleteCompany(int id)
        {
            var company = _context.Companies.Single(x => x.Id == id);

            if (company == null)
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            _context.Companies.Remove(company);
            _context.SaveChanges();

            return id;
        }

    }
}
