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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetAllCompaniesAsync()
        {
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompanyAsync(string id)
        {
            var company = companies.FirstOrDefault(company => company.Id == id);
            if (company != null)
            {
                return Ok(company);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Company>> UpdateCompany(string id, CompanyToUpsert companyToUpsert)
        {
            var company = companies.FirstOrDefault(company => company.Id == id);
            if (company != null)
            {
                company.Name = companyToUpsert.Name;
                return Ok(company);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("{companyId}/employees")]
        public async Task<ActionResult<Employee>> AddEmployee(string companyId, Employee employee)
        {
            var company = companies.FirstOrDefault(comp => comp.Id == companyId);
            if (company == null)
            {
                return NotFound();
            }

            if (company.Employees.Contains(employee))
            {
                return Conflict();
            }
            
            company.Employees.Add(employee);
            return Ok(employee);
        }

        [HttpGet("{companyId}/employees")]
        public async Task<ActionResult<Employee>> GetAllEmployees(string companyId)
        {
            var company = companies.FirstOrDefault(comp => comp.Id == companyId);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(company.Employees);
        }

        [HttpDelete]
        public async Task DeleteAll()
        {
            companies.Clear();
        }
    }
}
