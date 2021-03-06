﻿using System;
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
                HttpContext.Response.StatusCode = 404;
                return null;
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
            var editTaskGroup = _context.TaskAssignmentGroups.SingleOrDefault(x => x.Id == id);

            if (editTaskGroup == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            if (!taskAssignmentGroup.IsValid())
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

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

            if (!taskAssignmentGroup.IsValid())
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

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
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            _context.TaskAssignmentGroups.Remove(taskAssignmentGroup);
            _context.SaveChanges();

            return id;
        }

    }
}
