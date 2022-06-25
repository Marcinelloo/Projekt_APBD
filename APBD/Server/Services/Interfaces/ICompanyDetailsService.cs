using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APBD.Server.Models;
using APBD.Shared;

namespace APBD.Server.Services.Interfaces
{
    public interface ICompanyDetailsService
    {
        public Task<bool> AddCompanyToWatchList(DashboardTicker company, string userId);
        public Task<bool> RemoveFromWatchList(string ticker, string userId);
        public Task<DashboardTickerList> GetWatchedCompany(string userId);

    }
}

