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
    }
}
