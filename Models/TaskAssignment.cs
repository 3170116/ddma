using Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ddma.Models
{
    public class TaskAssignment
    {

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? Deadline { get; set; }

        public TaskAssignmentPriority Priority { get; set; }

        public TaskAssignmentStatus Status { get; set; }

        public int TaskAssignmentGroupId { get; set; }

        public virtual TaskAssignmentGroup TaskAssignmentGroup { get; set; }

        public virtual ICollection<TaskAssignmentUser> TaskAssignmentUsers { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<TaskLog> TaskLogs { get; set; }


        public void setStatus(int statusId)
        {
            switch (statusId)
            {
                case 0:
                    Status = TaskAssignmentStatus.IN_PROGRESS;
                    break;
                case 1:
                    Status = TaskAssignmentStatus.TO_BE_TESTED;
                    break;
                case 2:
                    Status = TaskAssignmentStatus.COMPLETED;
                    break;
                case 3:
                    Status = TaskAssignmentStatus.EXPIRED;
                    break;
            }
        }

        public void setPriority(int priority)
        {
            switch (priority)
            {
                case 0:
                    Priority = TaskAssignmentPriority.NONE;
                    break;
                case 1:
                    Priority = TaskAssignmentPriority.LOW;
                    break;
                case 2:
                    Priority = TaskAssignmentPriority.MEDIUM;
                    break;
                case 3:
                    Priority = TaskAssignmentPriority.HIGH;
                    break;
            }
        }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Title);
        }

    }
}
