using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBD.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<UserCompany> UserWatchedCompanies { get; set; }
    }
}

