﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ddma.Models
{
    public class Asset
    {

        [Key]
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int? LocationId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Company Company { get; set; }

        public virtual Location Location { get; set; }

        public virtual ICollection<TaskAssignmentAsset> TaskAssignmentAssets { get; set; } = new List<TaskAssignmentAsset>();

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Name);
        }

    }
}
