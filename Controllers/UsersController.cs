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

        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public User GetUser(int id)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id == id);

            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            return user;
        }

        /// <summary>
        /// Φέρνει πίσω τα task που έχει αναλάβει ο συγκεκριμένος χρήστης.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{id}/taskAssignments")]
        public IEnumerable<TaskAssignment> GetTaskAssignmentUser(int id)
        {
            return _context.TaskAssignmentUsers.Include("TaskAssignment").Where(x => x.UserId == id).Select(x => x.TaskAssignment).ToList();
        }

        /// <summary>
        /// Θα καλείται όταν ο χρήστης κάνει edit το προφίλ του ή θέλει να αλλάξει τα credentials του.
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
        /// Θα καλείται όταν κάνει ο χρήστης login.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public User GetUser(User user)
        {
            var loginUser = _context.Users.SingleOrDefault(x => x.Email == user.Email);

            if (loginUser == null || loginUser.PasswordHash != user.PasswordHash)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            return loginUser;
        }

        /// <summary>
        /// Θα καλείται όταν εγγράφεται νέος χρήστης.
        /// </summary>
        /// <returns></returns>
        [HttpPost("signUp")]
        public User PostUser(User user)
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
