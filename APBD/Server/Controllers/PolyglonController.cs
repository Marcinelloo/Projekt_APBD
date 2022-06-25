using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using APBD.Shared;
using APBD.Server.Services.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using APBD.Server.Models;

namespace APBD.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PolyglonController : ControllerBase
    {


        private readonly IPolyglon _service;


        public PolyglonController(IPolyglon service)
        {
            _service = service;

        }

        [HttpGet("symbol/{symbol}")]
        public async Task<DashboardSetchList> SearchCompanyBySymbol(string symbol)
        {

            return await _service.SearchCompanyBySymbol(symbol);
        }

        [HttpGet("ticker/{ticker}")]
        public async Task<DashboardTickerResult> GetCompanyDetailsByTicker(string ticker)
        {
            return await _service.GetCompanyBySymbol(ticker);
        }


        [HttpGet("stocks/{ticker}")]
        public async Task<StockDateInformationForDashboardDTO> GetStocksInformation(string ticker)
        {
            return await _service.GetStocksInformation(ticker);
        }
    }
}

