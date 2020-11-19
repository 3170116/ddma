using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ddma.Models
{
    public class TaskAssignmentAsset
    {

        [Key]
        public int Id { get; set; }

        public int TaskAssignmentId { get; set; }

        public int AssetId { get; set; }

        public int UserId { get; set; }

        public virtual Asset Asset { get; set; }

        public virtual TaskAssignment TaskAssignment { get; set; }

    }
}
