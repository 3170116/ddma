using ddma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddma.Summaries
{
    public class CompanySummary
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CompanySummary(Company company)
        {
            Id = company.Id;
            Name = company.Name;
        }
    }
}
