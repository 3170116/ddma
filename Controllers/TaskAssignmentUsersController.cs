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

        /// <summary>
        /// Φέρνει πίσω τα task που έχει αναλάβει ο συγκεκριμένος χρήστης.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("user/tasks/{userId}")]
        public IEnumerable<TaskAssignment> GetTaskAssignmentUser(int userId)
        {
            return _context.TaskAssignmentUsers.Include("TaskAssignment").Where(x => x.UserId == userId).Select(x => x.TaskAssignment).ToList();
        }

        /// <summary>
        /// Φέρνει πίσω τα tasks της εταιρείας.
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet("company/tasks/{companyId}")]
        public IEnumerable<TaskAssignment> GetTasks(int companyId)
        {
            //οι υπάλληλοι της εταιρείας
            var company = _context.Companies.Include("TaskAssignmentGroups.TaskAssignments").SingleOrDefault(x => x.Id == companyId);

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

        // POST: api/TaskAssignmentUsers
        /// <summary>
        /// Θα καλείται όταν κάνει ανάθεσει ο super-supervisor ή κάποιος supervisor ένα task σε έναν employee.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="taskAssignmentUser"></param>
        /// <returns></returns>
        [HttpPost("company/{companyId}")]
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
