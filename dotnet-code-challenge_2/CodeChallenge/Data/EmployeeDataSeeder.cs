using CodeChallenge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Data
{
    public class EmployeeDataSeeder
    {
        private EmployeeContext _employeeContext;
        private const String EMPLOYEE_SEED_DATA_FILE = "resources/EmployeeSeedData.json";

        public EmployeeDataSeeder(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }

        public async Task Seed()
        {
            if(!_employeeContext.Employees.Any())
            {
                List<Employee> employees = LoadEmployees();
                _employeeContext.Employees.AddRange(employees);

                await _employeeContext.SaveChangesAsync();
            }

            //testing 
            if (!_employeeContext.Compensations.Any())
            {
                
                var john = _employeeContext.Employees.First(e => e.EmployeeId == "16a596ae-edd3-4847-99fe-c4518e82c86f");
                var paul = _employeeContext.Employees.First(e => e.EmployeeId == "b7839309-3348-463b-a7e3-5de1c168beb3");

                _employeeContext.Compensations.AddRange(new List<Compensation>
                {
                    new Compensation
                    {
                        CompensationId = Guid.NewGuid().ToString(),
                        EmployeeId = john.EmployeeId,
                        Employee = john,
                        Salary = 80000,
                        EffectiveDate = DateTime.UtcNow
                    },
                    new Compensation
                    {
                        CompensationId = Guid.NewGuid().ToString(),
                        EmployeeId = paul.EmployeeId,
                        Employee = paul,
                        Salary = 50000,
                        EffectiveDate = DateTime.UtcNow
                    }
                });

                await _employeeContext.SaveChangesAsync();
            }
        }

        private List<Employee> LoadEmployees()
        {
            using (FileStream fs = new FileStream(EMPLOYEE_SEED_DATA_FILE, FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                List<Employee> employees = serializer.Deserialize<List<Employee>>(jr);
                FixUpReferences(employees);

                return employees;
            }
        }

        private void FixUpReferences(List<Employee> employees)
        {
            var employeeIdRefMap = from employee in employees
                                select new { Id = employee.EmployeeId, EmployeeRef = employee };

            employees.ForEach(employee =>
            {
                
                if (employee.DirectReports != null)
                {
                    var referencedEmployees = new List<Employee>(employee.DirectReports.Count);
                    employee.DirectReports.ForEach(report =>
                    {
                        var referencedEmployee = employeeIdRefMap.First(e => e.Id == report.EmployeeId).EmployeeRef;
                        referencedEmployees.Add(referencedEmployee);
                    });
                    employee.DirectReports = referencedEmployees;
                }
            });
        }
    }
}
