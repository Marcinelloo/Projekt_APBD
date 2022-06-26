using System;
using System.Threading.Tasks;
using APBD.Shared;

namespace APBD.Server.Services.Interfaces
{
    public interface IPolyglonService
    {

        public Task<DashboardSetchList> SearchCompanyBySymbol(string symbol);
        public Task<DashboardTickerResult> GetCompanyBySymbol(string symbol);
        public Task<StockDateInformationForDashboardDTO> GetStocksInformation(string ticker);

    }
}

