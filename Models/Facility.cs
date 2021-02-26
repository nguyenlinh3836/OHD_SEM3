using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OHD_SEM3.Models
{
    public class Facility
    {
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
