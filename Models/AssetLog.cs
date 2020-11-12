using Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ddma.Models
{
    public class AssetLog
    {
        [Key]
        public int Id { get; set; }

        public int AssetId { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public AssetLogType AssetLogType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public virtual Asset Asset { get; set; }

        public virtual User User { get; set; }
    }
}
