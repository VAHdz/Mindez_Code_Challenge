using System.Net;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using CodeChallenge.Models;
using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;


//Nested Report Testing in Notes file 
namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class ReportControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod] //Test: Employee with no reports -> Edge Case Test 
        public void GetReport_EmployeeWithNoReports_Returns_Zero()
        {
            var employeeId = "b7839309-3348-463b-a7e3-5de1c168beb3"; // Paul McCartney

            var getRequestTask = _httpClient.GetAsync($"api/report/{employeeId}");
            var response = getRequestTask.Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var report = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(0, report.NumberOfReports);
            Assert.AreEqual(employeeId, report.Employee.EmployeeId);
        }

       
        


        [TestMethod] //Test: Reporting Endpoint when invalid id -> Edge Case Test
        public void GetReport_EmployeeDoesNotExist_Returns_NotFound()
        {
      
            var employeeId = "invalid-id";
       
            var getRequestTask = _httpClient.GetAsync($"api/report/{employeeId}");
            var response = getRequestTask.Result;

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        
        
    }
}
