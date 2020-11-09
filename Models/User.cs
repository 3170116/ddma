using Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ddma.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public int? CompanyId { get; set; }

        public string Email { get; set; }

        public int PasswordHash { get; set; }

        public string NickName { get; set; }

        public UserRole RoleId { get; set; }

        public string TimeZone { get; set; }

        public Company Company { get; set; }

        public ICollection<TaskAssignmentUser> TaskAssignmentUsers { get; set; }

        public ICollection<UserNotification> UserNotifications { get; set; }

        public ICollection<TaskLog> TaskLogs { get; set; }


        public void SetRole(int roleId)
        {
            switch (roleId)
            {
                case 0:
                    RoleId = UserRole.EMPLOYEE;
                    break;
                case 1:
                    RoleId = UserRole.SUPERVISOR;
                    break;
                case 2:
                    RoleId = UserRole.SUPERSUPERVISOR;
                    break;
            }
        }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Email) && !String.IsNullOrEmpty(NickName);
        }

    }
}
