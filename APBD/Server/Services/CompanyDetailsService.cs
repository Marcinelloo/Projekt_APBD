using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using APBD.Server.Data;
using APBD.Server.Models;
using APBD.Server.Services.Interfaces;
using APBD.Shared;
using Microsoft.EntityFrameworkCore;

namespace APBD.Server.Services
{
    public class CompanyDetailsService : ICompanyDetailsService
    {

        private readonly ApplicationDbContext _applicationDbContext;
        public CompanyDetailsService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> AddCompanyToWatchList(DashboardTicker company, string userId)
        {
            try
            {
                var companyFromDatabase = await _applicationDbContext.Companies.Where(e => e.ticker == company.ticker).FirstOrDefaultAsync();

                if (companyFromDatabase is null)
                {
                    CompanyDetails companyDetails = new CompanyDetails()
                    {
                        ticker = company.ticker,
                        name = company.name,
                        sic_description = company.sic_description,
                        total_employees = company.total_employees,
                        locale = company.locale,
                        phone_number = company.phone_number,
                        branding = company.branding.icon_url,
                    };

                    await _applicationDbContext.Companies.AddAsync(companyDetails);
                    await _applicationDbContext.SaveChangesAsync();
                    companyFromDatabase = await _applicationDbContext.Companies.Where(e => e.ticker == company.ticker).FirstOrDefaultAsync();
                }
                Console.WriteLine(companyFromDatabase.IdWatchedCompany);

                UserCompany watchedCompany = new UserCompany()
                {
                    IdUser = userId,
                    IdWatchedCompany = companyFromDatabase.IdWatchedCompany,
                };

                await _applicationDbContext.WatchedCompany.AddAsync(watchedCompany);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<DashboardTickerList> GetWatchedCompany(string userId)
        {
            try
            {
                var watchedCompanies = await _applicationDbContext.WatchedCompany.Where(e => e.IdUser == userId).Select(e => e.WatchedCompany).ToListAsync();
                DashboardTickerList result = new DashboardTickerList();
                result.dashboardTickers = new List<DashboardTicker>();

                watchedCompanies.ForEach(company =>
               {
                   result.dashboardTickers.Add(new DashboardTicker
                   {
                       ticker = company.ticker,
                       name = company.name,
                       sic_description = company.sic_description,
                       total_employees = company.total_employees,
                       locale = company.locale,
                       phone_number = company.phone_number,
                       branding = new Icon()
                       {
                           icon_url = company.branding,
                       },
                   });
               });

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public async Task<bool> RemoveFromWatchList(string ticker, string userId)
        {
            try
            {

                var company = await _applicationDbContext.Companies.Where(e => e.ticker == ticker).FirstOrDefaultAsync();
                var userCompany = await _applicationDbContext.WatchedCompany.Where(e => e.IdUser == userId && e.IdWatchedCompany == company.IdWatchedCompany).FirstOrDefaultAsync();
                _applicationDbContext.WatchedCompany.Remove(userCompany);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        //public async Task<Da> GetComapny(string ticker)
        //{
        //    try
        //    {
        //        var track = new UserCompany() { IdWatchedCompany = id };
        //        _applicationDbContext.WatchedCompany.Remove(track);
        //        await _applicationDbContext.SaveChangesAsync();

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }
}

