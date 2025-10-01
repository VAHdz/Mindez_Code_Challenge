using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class ReportService : IReportService
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<ReportService> _logger;

        public ReportService(ILogger<ReportService> logger, IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            _logger = logger;
        }


        public ReportingStructure GetReportingStructure(string employeeId)
        {
            var employee = _employeeService.GetById(employeeId);
            if (employee == null) return null;

            
            int numberOfReports = CountReports(employee);
            return new ReportingStructure()
            {
                Employee = employee,
                NumberOfReports = numberOfReports
            };
        }

        private int CountReports(Employee employee)
        {
            //Error Check: Check for null values 
            if (employee == null || employee.DirectReports == null || !employee.DirectReports.Any())
                return 0;

           
            //Get the direct number of reports here 
            int totalReports = employee.DirectReports.Count;
            foreach (var employeeReport in employee.DirectReports) //SubReports of employees
            {
                Employee report = _employeeService.GetById(employeeReport.EmployeeId);
                totalReports += CountReports(report);
            }
            
            return totalReports;



            

        }

    
    }
}