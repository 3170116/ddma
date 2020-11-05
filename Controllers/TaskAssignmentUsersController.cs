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
