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
        public async Task<bool> AddComapnyToWatchList(DashboardTicker company)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _service.AddCompanyToWatchList(company, userId);
        }

        [HttpDelete("delete/{ticker}")]
        public async Task<bool> DeleteFromWatchList(string ticker)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _service.RemoveFromWatchList(ticker, userId);
        }

        [HttpGet]
        public async Task<DashboardTickerList> GetAllUserWatchedCompany()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _service.GetWatchedCompany(userId);
        }

    }
}

