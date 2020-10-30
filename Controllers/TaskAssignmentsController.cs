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
    public class TaskAssignmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskAssignmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TaskAssignments/5
        /// <summary>
        /// Θα καλείται όταν ο χρήστης θέλει να δει αναλυτικά ένα task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public TaskAssignment GetTaskAssignment(int id)
        {
            var taskAssignment = _context.TaskAssignments.Include("TaskAssignmentUsers.User").Include("Comments.User").SingleOrDefault(x => x.Id == id);

            if (taskAssignment == null)
            {
                return new TaskAssignment();
            }

            return taskAssignment;
        }

        // PUT: api/TaskAssignments/5
        /// <summary>
        /// Θα καλείται όταν θέλει να κάνει ο χρήστης edit ένα task.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskAssignment"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public TaskAssignment PutTaskAssignment(int id, TaskAssignment taskAssignment)
        {
            var editTask = _context.TaskAssignments.SingleOrDefault(x => x.Id == id);

            if (editTask == null)
            {
                return new TaskAssignment();
            }

            editTask.Title = taskAssignment.Title;
            editTask.Description = taskAssignment.Description;
            editTask.Priority = taskAssignment.Priority;
            editTask.Status = taskAssignment.Status;
            editTask.Deadline = taskAssignment.Deadline;

            _context.SaveChanges();

            return editTask;
        }

        // POST: api/TaskAssignments
        /// <summary>
        /// Θα καλείται όταν προσθέτει ο χρήστης ένα task.
        /// </summary>
        /// <param name="priorityId"></param>
        /// <param name="taskAssignment"></param>
        /// <returns></returns>
        [HttpPost("{priorityId}")]
        public TaskAssignment PostTaskAssignment(int priorityId, TaskAssignment taskAssignment)
        {
            taskAssignment.setPriority(priorityId);
            taskAssignment.setStatus(0);

            taskAssignment.CreatedAt = DateTime.UtcNow;

            _context.TaskAssignments.Add(taskAssignment);
            _context.SaveChanges();

            return taskAssignment;
        }

        // DELETE: api/TaskAssignments/5
        /// <summary>
        /// Θα καλείται όταν διαγράφει ο χρήστης ένα task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public int DeleteTaskAssignment(int id)
        {
            var taskAssignment = _context.TaskAssignments.SingleOrDefault(x => x.Id == id);

            if (taskAssignment == null)
            {
                return 0;
            }

            _context.TaskAssignments.Remove(taskAssignment);
            _context.SaveChanges();

            return id;
        }

    }
}
