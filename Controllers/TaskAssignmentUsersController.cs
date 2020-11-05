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
    public class TaskAssignmentUsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskAssignmentUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/TaskAssignmentUsers
        /// <summary>
        /// Θα καλείται όταν κάνει ανάθεσει ο super-supervisor ή κάποιος supervisor ένα task σε έναν employee.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="taskAssignmentUser"></param>
        /// <returns></returns>
        [HttpPost("{companyId}")]
        public int PostTaskAssignmentUser(int companyId, TaskAssignmentUser taskAssignmentUser)
        {

            //ελέγχουμε αν έχει γίνει ήδη assign αυτό το task σε αυτόν τον χρήστη
            var newTaskAssignmentUser = _context.TaskAssignmentUsers.SingleOrDefault(x => x.TaskAssignmentId == taskAssignmentUser.TaskAssignmentId && x.UserId == taskAssignmentUser.UserId);

            if (newTaskAssignmentUser != null)
            {
                return newTaskAssignmentUser.Id;
            }

            //ελέγχουμε αν ανήκει ο συγκεκριμένος χρήστης στην εταιρεία.
            var company = _context.Companies.Include("Users").SingleOrDefault(x => x.Id == companyId);

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

        // DELETE: api/TaskAssignmentUsers/5
        [HttpDelete("{id}")]
        public int DeleteTaskAssignmentUser(int id)
        {

            var taskAssignmentUser = _context.TaskAssignmentUsers.SingleOrDefault(x => x.Id == id);

            if (taskAssignmentUser == null)
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            _context.TaskAssignmentUsers.Remove(taskAssignmentUser);
            _context.SaveChanges();

            return id;

        }

    }
}
