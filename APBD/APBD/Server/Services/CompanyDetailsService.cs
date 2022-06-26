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
using Newtonsoft.Json;

namespace APBD.Server.Services
{
    public class CompanyDetailsService : ICompanyDetailsService
    {

        private readonly ApplicationDbContext _applicationDbContext;
        public CompanyDetailsService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> AddCompanyToWatchList(DashboardTrickerDTO company, string userId)
        {
            try
            {
                var companyFromDatabase = await _applicationDbContext.Companies.Where(e => e.ticker == company.CompanyDetailsInformation.results.ticker).FirstOrDefaultAsync();

                var lastDay = JsonConvert.SerializeObject(company.CompanyStockInformaion.day);
                var sevenDays = JsonConvert.SerializeObject(company.CompanyStockInformaion.sevenDaysStock);
                var threeMonths = JsonConvert.SerializeObject(company.CompanyStockInformaion.threeMonths);

                if (companyFromDatabase is null)
                {
                    CompanyDetails companyDetails = new CompanyDetails()
                    {
                        ticker = company.CompanyDetailsInformation.results.ticker,
                        name = company.CompanyDetailsInformation.results.name,
                        sic_description = company.CompanyDetailsInformation.results.sic_description,
                        total_employees = company.CompanyDetailsInformation.results.total_employees,
                        locale = company.CompanyDetailsInformation.results.locale,
                        phone_number = company.CompanyDetailsInformation.results.phone_number,
                        branding = company.CompanyDetailsInformation.results.branding.icon_url,
                        LastDayStocks = lastDay,
                        SevenDaysStocks = sevenDays,
                        ThreeMonthsStocks = threeMonths,
                    };

                    await _applicationDbContext.Companies.AddAsync(companyDetails);
                    await _applicationDbContext.SaveChangesAsync();
                    companyFromDatabase = await _applicationDbContext.Companies.Where(e => e.ticker == company.CompanyDetailsInformation.results.ticker).FirstOrDefaultAsync();
                }
                else
                {
                    companyFromDatabase.LastDayStocks = lastDay;
                    companyFromDatabase.SevenDaysStocks = sevenDays;
                    companyFromDatabase.ThreeMonthsStocks = threeMonths;
                    await _applicationDbContext.SaveChangesAsync();
                }

                var watchedComapny = await _applicationDbContext.WatchedCompany.Where(e => e.IdUser == userId && e.IdWatchedCompany == companyFromDatabase.IdWatchedCompany).FirstOrDefaultAsync();

                if (watchedComapny is null)
                {
                    UserCompany watchedCompany = new UserCompany()
                    {
                        IdUser = userId,
                        IdWatchedCompany = companyFromDatabase.IdWatchedCompany,
                    };

                    await _applicationDbContext.WatchedCompany.AddAsync(watchedCompany);
                    await _applicationDbContext.SaveChangesAsync();
                }

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


        public async Task<DashboardSetchList> SearchCompanyBySymbol(string symbol)
        {
            try
            {
                var searchedElementsFromDatabase = await _applicationDbContext.Companies.Where(e => e.ticker.Contains(symbol)).ToListAsync();

                var result = new DashboardSetchList();
                result.results = new List<DashboardSearch>();

                searchedElementsFromDatabase.ForEach(e =>
                {
                    var searchElement = new DashboardSearch()
                    {
                        ticker = e.ticker,
                        name = e.name,
                    };
                    result.results.Add(searchElement);
                });
                return result;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<DashboardTickerResult> GetCompanyBySymbol(string ticker)
        {
            try
            {
                var company = await _applicationDbContext.Companies.Where(e => e.ticker == ticker).FirstOrDefaultAsync();
                var result = new DashboardTickerResult();

                if (company is not null)
                {
                    result.results = new DashboardTicker()
                    {
                        branding = new Icon() { icon_url = company.branding },
                        total_employees = company.total_employees,
                        locale = company.locale,
                        name = company.name,
                        phone_number = company.phone_number,
                        sic_description = company.sic_description,
                        ticker = company.ticker,
                    };

                    return result;
                }
                else
                {
                    throw new Exception();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        public async Task<StockDateInformationForDashboardDTO> GetStocksInformation(string ticker)
        {
            try
            {

                var company = await _applicationDbContext.Companies.Where(e => e.ticker == ticker).FirstOrDefaultAsync();
                var result = new StockDateInformationForDashboardDTO();

                if (company is not null)
                {
                    var dayStock = JsonConvert.DeserializeObject<List<StockDateInformation>>(company.LastDayStocks);
                    var sevenDaysStock = JsonConvert.DeserializeObject<List<StockDateInformation>>(company.SevenDaysStocks);
                    var threeMonthsStock = JsonConvert.DeserializeObject<List<StockDateInformation>>(company.ThreeMonthsStocks);

                    return new StockDateInformationForDashboardDTO() { ticker = company.ticker, threeMonths = threeMonthsStock, day = dayStock, sevenDaysStock = sevenDaysStock, };
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


    }
}

