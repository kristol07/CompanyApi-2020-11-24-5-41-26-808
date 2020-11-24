using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CompanyApi;
using CompanyApi.Models;
using Microsoft.AspNetCore.Hosting;
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
    }
}
