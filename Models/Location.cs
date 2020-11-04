using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ddma.Models
{
    public class Location
    {

        [Key]
        public int Id { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string ZipCode { get; set; }

        public virtual Asset Asset { get; set; }

    }
}
