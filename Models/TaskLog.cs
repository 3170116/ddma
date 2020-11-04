using Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ddma.Models
{
    public class TaskLog
    {

        [Key]
        public int Id { get; set; }

        public int TaskAssignmentId { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public TaskUserType TaskUserType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public virtual TaskAssignment TaskAssignment { get; set; }

        public virtual User User { get; set; }
    }
}
