using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;

using CodeChallenge;           
using CodeChallenge.Models;    



namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static WebApplicationFactory<Program> _factory;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _factory = new WebApplicationFactory<Program>();
            _httpClient = _factory.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            _httpClient.Dispose();
            _factory.Dispose();
        }

        [TestMethod] //Test: Whether compensation was created 
        public async Task Create_Compensation_Returns_Created()
        {
            var compensation = new
            {
                employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                salary = 90000,
                effectiveDate = DateTime.UtcNow
            };

            var content = new StringContent(
                JsonSerializer.Serialize(compensation),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("/api/compensation", content);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, 
                $"Expected Created but got {response.StatusCode}");
        }

        [TestMethod] //Test: compensation returned for an employee
        public async Task Get_Compensation_By_EmployeeId_Returns_Ok()
        {
            var response = await _httpClient.GetAsync("/api/compensation/16a596ae-edd3-4847-99fe-c4518e82c86f");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }



        [TestMethod] //Test: null compensation 
        public async Task Create_NullCompensation_Returns_BadRequest()
        {
            var content = new StringContent("", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/compensation", content);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod] //Test: nonexisting employee
        public async Task Get_CompensationBy_NonExist_Employee_Returns_NotFound()
        {
            var response = await _httpClient.GetAsync("/api/compensation/invalid-id");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }





    }
}
