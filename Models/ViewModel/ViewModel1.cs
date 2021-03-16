using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OHD_SEM3.Models.ViewModel
{
    public class ViewModel1
    {
        public Request _requests { get; set; }
        public Facility _facilities { get; set; }   
        public IdentityRole _roles { get; set; }
        public IdentityUserRole<string> _userrole { get; set; }
        public IdentityUser _users { get; internal set; }
        public ICollection<User> Users { get; set; }
    }
}
