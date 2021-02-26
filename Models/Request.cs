using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OHD_SEM3.Models
{
    public class Request
    {
        public int RequestId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Remark { get; set; }
        public int FacilityId { get; set; }
        public Facility Facility { get; set; }
        public int? Assignee { get; set; }
        public int requestorId { get; set; }
        public User User { get; set; }       
    }
}
