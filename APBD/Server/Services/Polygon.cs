﻿using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using APBD.Server.Services.Interfaces;
using APBD.Shared;

namespace APBD.Server.Services
{
    public class Polygon : IPolyglon
    {

        private readonly string AccessKey = "BGECFGGCsrXSWU1QyALI_xWsV2vDlO4h";
        private readonly string Url1 = " https://api.polygon.io/v3/reference/tickers/";
        private readonly string Url2 = "  https://api.polygon.io/v2/aggs/ticker/";

        private HttpClient _httpClient;

        public Polygon(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<DashboardSetchList> SearchCompanyBySymbol(string symbol)
        {
            try
            {
                using var responseStream = await _httpClient.GetStreamAsync(SearchCompanyBySymbolUrl(symbol));
                var currentForecast = await JsonSerializer.DeserializeAsync<DashboardSetchList>(responseStream);

                return currentForecast;
            }
            catch (Exception)
            {
                return new DashboardSetchList();
            }

        }

        public async Task<DashboardTickerResult> GetCompanyBySymbol(string symbol)
        {
            try
            {
                using var responseStream = await _httpClient.GetStreamAsync(GetCompanyBySymbolUrl(symbol));
                var currentForecast = await JsonSerializer.DeserializeAsync<DashboardTickerResult>(responseStream);


                return currentForecast;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new DashboardTickerResult();
            }

        }

        private string SearchCompanyBySymbolUrl(string symbol)

        {

            return Url1 + "?search=" + symbol + "&active=true&sort=ticker&order=asc&limit=10&apiKey=" + AccessKey;
        }

        private string GetCompanyBySymbolUrl(string symbol)
        {

            return Url1 + symbol + "?apiKey=" + AccessKey;
        }

        public async Task<StockDateInformationForDashboardDTO> GetStocksInformation(string ticker)
        {
            try
            {
                var startDate = DateTime.Now;
                var endDate = DateTime.Now;


                if (startDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    startDate = startDate.AddDays(-1);
                }
                else if (startDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    startDate = startDate.AddDays(-2);
                }
                if (endDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    endDate = endDate.AddDays(-1);
                }
                else if (startDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    endDate = endDate.AddDays(-2);
                }


                using var responseStreamDay = await _httpClient.GetStreamAsync(GetStocksInformationUrlForThreeMonths(ticker, startDate, endDate));
                using var responseStream90Days = await _httpClient.GetStreamAsync(GetStocksInformationUrlForDay(ticker, startDate, endDate));
                using var responseStream7Days = await _httpClient.GetStreamAsync(GetStocksInformationUrlFor7Days(ticker, startDate, endDate));

                var dayStock = await JsonSerializer.DeserializeAsync<StockDateInformationDTO>(responseStreamDay);
                var sevenDaysStock = await JsonSerializer.DeserializeAsync<StockDateInformationDTO>(responseStream7Days);
                var threeMonthsStock = await JsonSerializer.DeserializeAsync<StockDateInformationDTO>(responseStream90Days);

                return new StockDateInformationForDashboardDTO() { ticker = dayStock.ticker, threeMonths = threeMonthsStock.results, day = dayStock.results, sevenDaysStock = sevenDaysStock.results };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new StockDateInformationForDashboardDTO();
            }
        }

        private string GetStocksInformationUrlForDay(string symbol, DateTime startDate, DateTime endDate)
        {

            return Url2 + symbol + "/range/1/minute/" + startDate.ToString("yyyy-MM-dd") + "/" + endDate.ToString("yyyy-MM-dd") + "?adjusted=true&sort=asc&apiKey=" + AccessKey;
        }

        private string GetStocksInformationUrlFor7Days(string symbol, DateTime startDate, DateTime endDate)
        {
            return Url2 + symbol + "/range/1/hour/" + startDate.AddDays(-7).ToString("yyyy-MM-dd") + "/" + endDate.ToString("yyyy-MM-dd") + "?adjusted=true&sort=asc&apiKey=" + AccessKey;
        }

        private string GetStocksInformationUrlForThreeMonths(string symbol, DateTime startDate, DateTime endDate)
        {
            return Url2 + symbol + "/range/1/day/" + startDate.AddDays(-90).ToString("yyyy-MM-dd") + "/" + endDate.ToString("yyyy-MM-dd") + "?adjusted=true&sort=asc&apiKey=" + AccessKey;
        }

    }
}
