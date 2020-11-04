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
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Comments/5
        /// <summary>
        /// Φέρνει τα comments αυτού του task.
        /// </summary>
        /// <param name="taskAssignemntId"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IEnumerable<Comment> GetComments(int taskAssignemntId)
        {
            var comments = _context.Comments.Where(x => x.TaskAssignmentId == taskAssignemntId).ToList();

            return comments;
        }

        // POST: api/Comments
        /// <summary>
        /// Θα καλείται όταν ο χρήστης προσθέτει ένα comment σε ένα task.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost]
        public Comment PostComment(Comment comment)
        {

            if (!comment.IsValid())
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return comment;

        }

        // DELETE: api/Comments/5
        /// <summary>
        /// Θα καλείται όταν ο super-supervisor ή κάποιος supervisor διαγράφει ένα comment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public int DeleteComment(int id)
        {
            var comment = _context.Comments.SingleOrDefault(x => x.Id == id);

            if (comment == null)
            {
                HttpContext.Response.StatusCode = 404;
                return 0;
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return id;
        }

    }
}
