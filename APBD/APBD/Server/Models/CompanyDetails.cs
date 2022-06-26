using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using APBD.Shared;

namespace APBD.Server.Models
{
    public class CompanyDetails
    {
        public int IdWatchedCompany { get; set; }
        public string ticker { get; set; }
        public string name { get; set; }
        public string sic_description { get; set; }
        public int total_employees { get; set; }
        public string locale { get; set; }
        public string phone_number { get; set; }
        public string branding { get; set; }

        [Column(TypeName = "Text")]
        public string LastDayStocks { get; set; }

        [Column(TypeName = "Text")]
        public string SevenDaysStocks { get; set; }

        [Column(TypeName = "Text")]
        public string ThreeMonthsStocks { get; set; }

        public virtual ICollection<UserCompany> UserWatchedCompanies { get; set; }

    }
}

