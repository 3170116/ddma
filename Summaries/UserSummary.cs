using ddma.Models;
using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddma.Summaries
{
    public class UserSummary
    {
        public int Id { get; set; }

        public int? CompanyId { get; set; }

        public string Email { get; set; }

        public string NickName { get; set; }

        public UserRole RoleId { get; set; }

        public string TimeZone { get; set; }

        public UserSummary(User user)
        {
            Id = user.Id;
            CompanyId = user.CompanyId;
            Email = user.Email;
            NickName = user.NickName;
            RoleId = user.RoleId;
            TimeZone = user.TimeZone;
        }
    }
}
