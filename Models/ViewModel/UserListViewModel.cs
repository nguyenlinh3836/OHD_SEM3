using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OHD_SEM3.Models.ViewModel
{
    public class UserListViewModel
    {
        public int TotalUser { get; set; }
        public int TotalRequest { get; set; }
        public int TotalFaccilies { get; set; }
        public int TotalPending { get; set; }
        
    }

    public class ListUserRole
    {
        public User User { get; set; }
        public IdentityRole Role { get; set; }
    }
       
}
