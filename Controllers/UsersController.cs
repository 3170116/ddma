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
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Επιστρέφει τη λίστα με τους εργαζόμενους της εταιρείας.
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet("company/{companyId}")]
        public IEnumerable<User> GetUsers(int companyId)
        {
            var comapny = _context.Companies.Include("Users").SingleOrDefault(x => x.Id == companyId);

            if (comapny == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            return comapny.Users.ToList();
        }

        /// <summary>
        /// Θα καλείται όταν κάνει ο χρήστης login.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("{email}")]
        public Company GetUser(string email, User user)
        {
            user = _context.Users.SingleOrDefault(x => x.Email == email && x.PasswordHash == user.PasswordHash);

            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            var company = _context.Companies.Include("Users").Include("TaskAssignmentGroups").SingleOrDefault(x => x.Id == user.CompanyId);

            if (company == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            return company;
        }

        /// <summary>
        /// Θα καλείται όταν ο employee κάνει edit το προφίλ του ή θέλει να αλλάξει τα credentials του.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public User PutUser(int id, User user)
        {

            if (!user.IsValid())
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            var editUser = _context.Users.SingleOrDefault(x => x.Id == id);

            if (editUser == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            editUser.PasswordHash = user.PasswordHash;
            editUser.NickName = user.NickName;

            _context.SaveChanges();

            return editUser;

        }

        /// <summary>
        /// Θα καλείται όταν εγγράφεται νέος χρήστης.
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        [HttpPost("{roleId}")]
        public User PostUser(int roleId, User user)
        {

            if (!user.IsValid())
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            var company = _context.Companies.Include(x => x.Users).Single(x => x.Id == user.CompanyId);

            if (company == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            if (_context.Users.Any(x => x.Email == user.Email))
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            user.SetRole(roleId);

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;

        }

        /// <summary>
        /// Θα καλείται όταν διαγράφει έναν υπάλληλο ο super-supervisor.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public int DeleteEmployee(int id)
        {

            var user = _context.Users.SingleOrDefault(x => x.Id == id);

            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return id;

        }

    }
}
