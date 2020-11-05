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

        /// <summary>
        /// Επιστρέφει τη λίστα με τους εργαζόμενους της εταιρείας.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // PUT: /companies/{id}/users
        [HttpGet("{id}/users")]
        public IEnumerable<User> GetUsers(int id)
        {
            var company = _context.Companies.Include(x => x.Users).SingleOrDefault(x => x.Id == id);

            if (company == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            return company.Users.ToList();
        }

        // PUT: /companies/{id}/taskAssignmentGroups
        [HttpGet("{id}/taskAssignmentGroups")]
        public IEnumerable<TaskAssignmentGroup> GetTaskAssignmentGroups(int id)
        {
            var company = _context.Companies.Include(x => x.TaskAssignmentGroups).SingleOrDefault(x => x.Id == id);

            if (company == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            return company.TaskAssignmentGroups.ToList();
        }

        /// <summary>
        /// Φέρνει πίσω τα tasks της εταιρείας.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/tasksAssignments")]
        public IEnumerable<TaskAssignment> GetTasksAssignments(int id)
        {
            //οι υπάλληλοι της εταιρείας
            var company = _context.Companies.Include("TaskAssignmentGroups.TaskAssignments").SingleOrDefault(x => x.Id == id);

            if (company == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            IList<TaskAssignment> result = new List<TaskAssignment>();

            foreach (var group in company.TaskAssignmentGroups)
            {
                foreach (var taskAssignment in group.TaskAssignments.ToList())
                {
                    result.Add(taskAssignment);
                }
            }

            return result;
        }

        /// <summary>
        /// Επιστρέφει τα assets της εταιρίας.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/assets")]
        public IEnumerable<Asset> GetAssets(int id)
        {
            return _context.Assets.Include(x => x.Location).Where(x => x.CompanyId == id).ToList();
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

        // POST: api/1/TaskAssignmentUsers
        /// <summary>
        /// Θα καλείται όταν κάνει ανάθεσει ο super-supervisor ή κάποιος supervisor ένα task σε έναν employee.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskAssignmentUser"></param>
        /// <returns></returns>
        [HttpPost("{id}/TaskAssignmentUsers")]
        public int PostTaskAssignmentUser(int id, TaskAssignmentUser taskAssignmentUser)
        {

            //ελέγχουμε αν έχει γίνει ήδη assign αυτό το task σε αυτόν τον χρήστη
            var newTaskAssignmentUser = _context.TaskAssignmentUsers.SingleOrDefault(x => x.TaskAssignmentId == taskAssignmentUser.TaskAssignmentId && x.UserId == taskAssignmentUser.UserId);

            if (newTaskAssignmentUser != null)
            {
                HttpContext.Response.StatusCode = 400;
                return newTaskAssignmentUser.Id;
            }

            //ελέγχουμε αν ανήκει ο συγκεκριμένος χρήστης στην εταιρεία.
            var company = _context.Companies.Include("Users").SingleOrDefault(x => x.Id == id);

            if (company == null || !company.Users.Any(x => x.Id == taskAssignmentUser.UserId))
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }


            taskAssignmentUser.AssignedAt = DateTime.UtcNow;

            _context.TaskAssignmentUsers.Add(taskAssignmentUser);
            _context.SaveChanges();


            return taskAssignmentUser.Id;

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
