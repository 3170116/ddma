﻿using Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ddma.Models
{
    public class UserNotification
    {

        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public NotificationType NotificationType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; }

    }
}
