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
    public class TaskAssignmentGroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskAssignmentGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Φέρνει τα assignment groups της εταιρείας.
        /// </summary>
        /// <returns></returns>
        [HttpGet("company/{companyId}")]
        public IEnumerable<TaskAssignmentGroup> GetTaskAssignmentGroups(int companyId)
        {
            return _context.TaskAssignmentGroups.Where(x => x.CompanyId == companyId).ToList();
        }

        // GET: api/TaskAssignmentGroups/5
        /// <summary>
        /// Θα καλείται όταν ο χρήστης θέλει να δει τα task ενός task group.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public TaskAssignmentGroup GetTaskAssignmentGroup(int id)
        {
            var taskAssignmentGroup = _context.TaskAssignmentGroups.Include("TaskAssignments").SingleOrDefault(x => x.Id == id);

            if (taskAssignmentGroup == null)
            {
                return new TaskAssignmentGroup();
            }

            return taskAssignmentGroup;
        }

        // PUT: api/TaskAssignmentGroups/5
        /// <summary>
        /// Θα καλείται όταν ο χρήστης κάνει edit ένα task group.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskAssignmentGroup"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public TaskAssignmentGroup PutTaskAssignmentGroup(int id, TaskAssignmentGroup taskAssignmentGroup)
        {
            var editTaskGroup = _context.TaskAssignmentGroups.Single(x => x.Id == id);

            editTaskGroup.Name = taskAssignmentGroup.Name;
            _context.SaveChanges();

            return editTaskGroup;
        }

        // POST: api/TaskAssignmentGroups
        /// <summary>
        /// Θα καλείται όταν ο χρήστης προσθέτει ένα task group.
        /// </summary>
        /// <param name="taskAssignmentGroup"></param>
        /// <returns></returns>
        [HttpPost]
        public TaskAssignmentGroup PostTaskAssignmentGroup(TaskAssignmentGroup taskAssignmentGroup)
        {
            _context.TaskAssignmentGroups.Add(taskAssignmentGroup);
            _context.SaveChanges();

            return taskAssignmentGroup;
        }

        // DELETE: api/TaskAssignmentGroups/5
        [HttpDelete("{id}")]
        public int DeleteTaskAssignmentGroup(int id)
        {
            var taskAssignmentGroup = _context.TaskAssignmentGroups.SingleOrDefault(x => x.Id == id);

            if (taskAssignmentGroup == null)
            {
                return 0;
            }

            _context.TaskAssignmentGroups.Remove(taskAssignmentGroup);
            _context.SaveChanges();

            return id;
        }

    }
}
