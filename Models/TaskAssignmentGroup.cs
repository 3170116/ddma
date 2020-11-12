using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ddma.Models
{
    public class TaskAssignmentGroup
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Name);
        }
    }
}
