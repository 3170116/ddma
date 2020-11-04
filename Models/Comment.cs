using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ddma.Models
{
    public class Comment
    {

        [Key]
        public int Id { get; set; }

        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }

        public int TaskAssignmentId { get; set; }

        public virtual User User { get; set; }

        public virtual TaskAssignment TaskAssignment { get; set; }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Body);
        }

    }
}
