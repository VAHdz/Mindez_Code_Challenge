using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/report")]
    public class ReportController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IReportService _reportService;

        public ReportController(ILogger<ReportController> logger, IReportService reportingService)
        {
            _logger = logger;
            _reportService = reportingService;
        }

        [HttpGet("{id}")]
        public IActionResult GetReports(string id)
        {

            _logger.LogDebug($"Received reporting structure request for employee '{id}'");

            ReportingStructure reportingStructure = _reportService.GetReportingStructure(id);
            if (reportingStructure == null) //Error Check: If no report struct found for an employeeId
                return NotFound();


            //Pretty print the report with employee information and list of direct ids and total reports
            var simplifiedReport = new
            {
                employee = new
                {
                    employeeId = reportingStructure.Employee.EmployeeId,
                    firstName = reportingStructure.Employee.FirstName,
                    lastName = reportingStructure.Employee.LastName,
                    position = reportingStructure.Employee.Position,
                    department = reportingStructure.Employee.Department,
                    directReports = reportingStructure.Employee.DirectReports?.Select(r => r.EmployeeId).ToList()
                },
                numberOfReports = reportingStructure.NumberOfReports
            };


            return Ok(simplifiedReport);
        }

       
    

    }
}