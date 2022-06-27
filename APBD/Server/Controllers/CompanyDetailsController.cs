using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using APBD.Shared;
using APBD.Server.Services.Interfaces;
using APBD.Server.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace APBD.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CompanyDetailsController : ControllerBase
    {


        private readonly ICompanyDetailsService _service;

        public CompanyDetailsController(ICompanyDetailsService service)
        {
            _service = service;
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddComapnyToWatchList([FromBody] DashboardTrickerDTO company)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.AddCompanyToWatchList(company, userId);

            if (result)
                return Ok();

            return BadRequest();
        }

        [HttpDelete("delete/{ticker}")]
        public async Task<ActionResult> DeleteFromWatchList(string ticker)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.RemoveFromWatchList(ticker, userId);

            if (result)
                return Ok();

            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<DashboardTickerList>> GetAllUserWatchedCompany()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _service.GetWatchedCompany(userId);

            if (result is not null)
                return Ok(result);

            return NotFound();
        }

        [HttpGet("companies/{symbol}")]
        public async Task<ActionResult<DashboardSetchList>> SearchCompanyBySymbol(string symbol)
        {


            var result = await _service.SearchCompanyBySymbol(symbol);

            if (result is not null)
            {
                return Ok(result);
            }

            return NotFound();
        }


        [HttpGet("ticker/{ticker}")]
        public async Task<ActionResult<DashboardTrickerDTO>> GetCompanyDetailsByTicker(string ticker)
        {
            var result = new DashboardTrickerDTO();

            var companyInformation = await _service.GetCompanyBySymbol(ticker);
            var stocksInformation = await _service.GetStocksInformation(ticker);


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

