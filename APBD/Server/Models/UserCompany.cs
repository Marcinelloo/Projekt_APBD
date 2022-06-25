using System;
namespace APBD.Server.Models
{
    public class UserCompany
    {
        public string IdUser { get; set; }
        public int IdWatchedCompany { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual CompanyDetails WatchedCompany { get; set; }
    }
}

