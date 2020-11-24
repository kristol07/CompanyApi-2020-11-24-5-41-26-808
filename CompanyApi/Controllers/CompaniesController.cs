using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompaniesController : ControllerBase
    {
        private static IList<Company> companies = new List<Company>();

        [HttpPost]
        public async Task<ActionResult<Company>> AddCompany(Company company)
        {
            if (companies.Any(comp => comp.Id == company.Id))
            {
                return Conflict();
            }

            companies.Add(company);
            return Ok(company);
        }
    }
}
