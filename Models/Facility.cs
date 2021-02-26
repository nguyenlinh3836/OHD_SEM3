using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OHDProject.Models
{
    public class Facility
    {
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
