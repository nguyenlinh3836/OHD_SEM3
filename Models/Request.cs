using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OHDProject.Models
{
    public class Request
    {
        public int RequestId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Remark { get; set; }
        public int FacilityId { get; set; }
        public Facility Facility { get; set; }
        public int? Assignee { get; set; }
        public int requestorId { get; set; }
        public Account Account { get; set; }       
    }
}
