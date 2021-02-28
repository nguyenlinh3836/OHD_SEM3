using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OHD_SEM3.Models.ViewModel
{
    public class AssigneeViewModel
    {
        public IdentityRole Role { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
