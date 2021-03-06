﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ddma.Models
{
    public class Company
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();

        public virtual ICollection<TaskAssignmentGroup> TaskAssignmentGroups { get; set; } = new List<TaskAssignmentGroup>();

        public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();

        public User GetSuperSupervisor()
        {
            return Users.Single(x => x.RoleId == Enums.UserRole.SUPERSUPERVISOR);
        }

        public IList<User> GetSupervisors()
        {
            return Users.Where(x => x.RoleId == Enums.UserRole.SUPERVISOR).ToList();
        }

        public IList<User> GetEmployees()
        {
            return Users.Where(x => x.RoleId == Enums.UserRole.EMPLOYEE).ToList();
        }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Name);
        }

        public IList<TaskAssignment> GetTaskAssignments()
        {

            IList<TaskAssignment> result = new List<TaskAssignment>();

            foreach (var group in TaskAssignmentGroups)
            {
                foreach (var taskAssignment in group.TaskAssignments.ToList())
                {
                    result.Add(taskAssignment);
                }
            }

            return result;
        }

    }
}
