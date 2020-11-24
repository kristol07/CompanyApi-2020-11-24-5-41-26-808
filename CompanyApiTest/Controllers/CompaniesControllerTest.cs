using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CompanyApi;
using CompanyApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace CompanyApiTest.Controllers
{
    public class CompaniesControllerTest
    {
        private HttpClient client;
        public CompaniesControllerTest()
        {
            TestServer server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            client = server.CreateClient();
            client.DeleteAsync("/companies");
        }

        [Fact]
        public async Task Should_add_new_company_if_not_existed()
        {
            var company = new Company("comp1");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/companies", requestBody);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualCompany = JsonConvert.DeserializeObject<Company>(responseString);
            Assert.Equal(company, actualCompany);
        }

        [Fact]
        public async Task Should_not_add_new_company_if_existed()
        {
            var company = new Company("comp1");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            await client.PostAsync($"/companies", requestBody);
            var response = await client.PostAsync($"/companies", requestBody);

            Assert.Equal(StatusCodes.Status409Conflict, (int)response.StatusCode);
        }

        [Fact]
        public async Task Should_return_all_companies_when_get_all()
        {
            var company = new Company("comp1");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync($"/companies", requestBody);

            var response = await client.GetAsync("/companies");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualCompanies = JsonConvert.DeserializeObject<IList<Company>>(responseString);
            Assert.Equal(new List<Company>() { company }, actualCompanies);
        }

        [Fact]
        public async Task Should_return_companies_in_pages_when_given_pagination()
        {
            var company = new Company("comp1");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync($"/companies", requestBody);

            var response = await client.GetAsync("/companies?limit=20&offset=0");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualCompanies = JsonConvert.DeserializeObject<IList<Company>>(responseString);
            Assert.Equal(new List<Company>() { company }, actualCompanies);

            response = await client.GetAsync("/companies?limit=20&offset=20");

            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            actualCompanies = JsonConvert.DeserializeObject<IList<Company>>(responseString);
            Assert.Equal(0, actualCompanies.Count);
        }

        [Fact]
        public async Task Should_return_specific_company_when_get_company_with_id()
        {
            var company = new Company("comp1");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync($"/companies", requestBody);

            var response = await client.GetAsync($"/companies/{company.Id}");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualCompany = JsonConvert.DeserializeObject<Company>(responseString);
            Assert.Equal(company, actualCompany);
        }

        [Fact]
        public async Task Should_return_updated_company_when_update_existing_company()
        {
            var company = new Company("comp1");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync($"/companies", requestBody);
            var companyToUpdate = new CompanyToUpsert("comp2");
            string requestToUpdate = JsonConvert.SerializeObject(companyToUpdate);
            StringContent requestBodyToUpdate = new StringContent(requestToUpdate, Encoding.UTF8, "application/json");
            var response = await client.PatchAsync($"/companies/{company.Id}", requestBodyToUpdate);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualCompany = JsonConvert.DeserializeObject<Company>(responseString);
            Assert.Equal(company.Id, actualCompany.Id);
            Assert.Equal(companyToUpdate.Name, actualCompany.Name);
        }

        [Fact]
        public async Task Should_add_new_employee_if_not_existed()
        {
            var company = new Company("comp1");
            string companyRequest = JsonConvert.SerializeObject(company);
            StringContent companyRequestBody = new StringContent(companyRequest, Encoding.UTF8, "application/json");
            await client.PostAsync($"/companies", companyRequestBody);

            var employee = new Employee("employ1", 100);
            string request = JsonConvert.SerializeObject(employee);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/companies/{company.Id}/employees", requestBody);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualEmployee = JsonConvert.DeserializeObject<Employee>(responseString);
            Assert.Equal(employee, actualEmployee);
        }

        [Fact]
        public async Task Should_return_all_employees_when_get_employees_under_specific_company()
        {
            var company = new Company("comp1");
            string companyRequest = JsonConvert.SerializeObject(company);
            StringContent companyRequestBody = new StringContent(companyRequest, Encoding.UTF8, "application/json");
            await client.PostAsync($"/companies", companyRequestBody);
            var employee = new Employee("employ1", 100);
            string employeeRequest = JsonConvert.SerializeObject(employee);
            StringContent employeeRequestBody = new StringContent(employeeRequest, Encoding.UTF8, "application/json");
            await client.PostAsync($"/companies/{company.Id}/employees", employeeRequestBody);

            var response = await client.GetAsync($"/companies/{company.Id}/employees");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualEmployees = JsonConvert.DeserializeObject<IList<Employee>>(responseString);
            Assert.Equal(new List<Employee>() { employee }, actualEmployees);
        }

        [Fact]
        public async Task Should_return_updated_employee_when_update_existing_employee()
        {
            var company = new Company("comp1");
            string companyRequest = JsonConvert.SerializeObject(company);
            StringContent companyRequestBody = new StringContent(companyRequest, Encoding.UTF8, "application/json");
            await client.PostAsync($"/companies", companyRequestBody);
            var employee = new Employee("employ1", 100);
            string employeeRequest = JsonConvert.SerializeObject(employee);
            StringContent employeeRequestBody = new StringContent(employeeRequest, Encoding.UTF8, "application/json");
            await client.PostAsync($"/companies/{company.Id}/employees", employeeRequestBody);

            var employeeToUpsert = new EmployeeToUpsert("employ2", 200);
            string newEmployeeRequest = JsonConvert.SerializeObject(employeeToUpsert);
            StringContent newEmployeeRequestBody = new StringContent(newEmployeeRequest, Encoding.UTF8, "application/json");

            var response =
                await client.PatchAsync($"/companies/{company.Id}/employees/{employee.Id}", newEmployeeRequestBody);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualEmployee = JsonConvert.DeserializeObject<Employee>(responseString);
            Assert.Equal(employee.Id, actualEmployee.Id);
            Assert.Equal(employeeToUpsert.Name, actualEmployee.Name);
            Assert.Equal(employeeToUpsert.Salary, actualEmployee.Salary);
        }

        [Fact]
        public async Task Should_delete_employee_when_delete_employee()
        {
            var company = new Company("comp1");
            string companyRequest = JsonConvert.SerializeObject(company);
            StringContent companyRequestBody = new StringContent(companyRequest, Encoding.UTF8, "application/json");
            await client.PostAsync($"/companies", companyRequestBody);
            var employee = new Employee("employ1", 100);
            string employeeRequest = JsonConvert.SerializeObject(employee);
            StringContent employeeRequestBody = new StringContent(employeeRequest, Encoding.UTF8, "application/json");
            await client.PostAsync($"/companies/{company.Id}/employees", employeeRequestBody);

            var response = await client.DeleteAsync($"/companies/{company.Id}/employees/{employee.Id}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
        }

        [Fact]
        public async Task Should_delete_company_when_delete_company()
        {
            var company = new Company("comp1");
            string companyRequest = JsonConvert.SerializeObject(company);
            StringContent companyRequestBody = new StringContent(companyRequest, Encoding.UTF8, "application/json");
            await client.PostAsync($"/companies", companyRequestBody);
            var employee = new Employee("employ1", 100);
            string employeeRequest = JsonConvert.SerializeObject(employee);
            StringContent employeeRequestBody = new StringContent(employeeRequest, Encoding.UTF8, "application/json");
            await client.PostAsync($"/companies/{company.Id}/employees", employeeRequestBody);

            var response = await client.DeleteAsync($"/companies/{company.Id}");
            response.EnsureSuccessStatusCode();
            Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
            var companiesResponse = await client.GetAsync("/companies");
            var companiesResponseString = await companiesResponse.Content.ReadAsStringAsync();
            var allCompanies = JsonConvert.DeserializeObject<IList<Company>>(companiesResponseString);
            Assert.Equal(0, allCompanies.Count);
        }
    }
}
