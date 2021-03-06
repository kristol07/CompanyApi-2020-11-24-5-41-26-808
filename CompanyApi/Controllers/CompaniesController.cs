﻿using System;
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
        public async Task<ActionResult<Company>> AddCompanyAsync(Company company)
        {
            if (companies.Any(comp => comp.Id == company.Id))
            {
                return Conflict();
            }

            companies.Add(company);
            return Ok(company);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetAllCompaniesAsync(int? limit, int? offset)
        {
            if (limit.HasValue && offset.HasValue)
            {
                return Ok(companies.Where(company =>
                {
                    var index = companies.IndexOf(company);
                    return index >= offset && index < (offset + limit);
                }));
            }

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
        public async Task<ActionResult<Company>> UpdateCompanyAsync(string id, CompanyToUpsert companyToUpsert)
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
        public async Task<ActionResult<Employee>> AddEmployeeAsync(string companyId, Employee employee)
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
        public async Task<ActionResult<Employee>> GetAllEmployeesAsync(string companyId)
        {
            var company = companies.FirstOrDefault(comp => comp.Id == companyId);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(company.Employees);
        }

        [HttpPatch("{companyId}/employees/{employeeId}")]
        public async Task<ActionResult<Employee>> GetAllEmployeesAsync(string companyId, string employeeId, EmployeeToUpsert employeeToUpsert)
        {
            var company = companies.FirstOrDefault(comp => comp.Id == companyId);
            if (company == null)
            {
                return NotFound();
            }

            var employee = company.Employees.FirstOrDefault(employee => employee.Id == employeeId);
            if (employee == null)
            {
                return NotFound();
            }

            employee.Name = employeeToUpsert.Name;
            employee.Salary = employeeToUpsert.Salary;
            return Ok(employee);
        }

        [HttpDelete("{companyId}/employees/{employeeId}")]
        public async Task<ActionResult> DeleteEmployeeAsync(string companyId, string employeeId)
        {
            var company = companies.FirstOrDefault(comp => comp.Id == companyId);
            if (company == null)
            {
                return NotFound();
            }

            var employee = company.Employees.FirstOrDefault(employee => employee.Id == employeeId);
            if (employee == null)
            {
                return NotFound();
            }

            company.Employees.Remove(employee);
            return NoContent();
        }

        [HttpDelete("{companyId}")]
        public async Task<ActionResult> DeleteCompanyAsync(string companyId)
        {
            var company = companies.FirstOrDefault(comp => comp.Id == companyId);
            if (company == null)
            {
                return NotFound();
            }

            companies.Remove(company);
            return NoContent();
        }

        [HttpDelete]
        public async Task DeleteAllAsync()
        {
            companies.Clear();
        }
    }
}
