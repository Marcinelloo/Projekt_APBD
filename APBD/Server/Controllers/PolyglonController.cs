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


        private readonly IPolyglonService _polyglonService;


        public PolyglonController(IPolyglonService service)
        {
            _polyglonService = service;

        }

        [HttpGet("symbol/{symbol}")]
        public async Task<ActionResult<DashboardSetchList>> SearchCompanyBySymbol(string symbol)
        {

            var searchedCompany = await _polyglonService.SearchCompanyBySymbol(symbol);

            if (searchedCompany is not null)
            {
                return Ok(searchedCompany);
            }

            return NotFound();
        }

        [HttpGet("ticker/{ticker}")]
        public async Task<ActionResult<DashboardTrickerDTO>> GetCompanyDetailsByTicker(string ticker)
        {
            var result = new DashboardTrickerDTO();

            var companyInformation = await _polyglonService.GetCompanyBySymbol(ticker);
            var stocksInformation = await _polyglonService.GetStocksInformation(ticker);


            if (stocksInformation is not null && companyInformation is not null)
            {
                result.CompanyStockInformaion = stocksInformation;
                result.CompanyDetailsInformation = companyInformation;
                return Ok(result);
            }

            return NotFound();
        }
    }
}

