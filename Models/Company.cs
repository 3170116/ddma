using System;
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

        public string TimeZone { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<TaskAssignmentGroup> TaskAssignmentGroups { get; set; }

        public User GetSuperSupervisor()
        {
            return Users.Single(x => x.RoleId == Enums.UserRole.SUPERSUPERVISOR);
        }

        public IList<User> GetEmployees()
        {
            return Users.Where(x => x.RoleId == Enums.UserRole.EMPLOYEE).ToList();
        }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(TimeZone);
        }

    }
}
