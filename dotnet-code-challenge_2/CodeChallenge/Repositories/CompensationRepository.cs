using System;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using CodeChallenge.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            if (string.IsNullOrEmpty(compensation.CompensationId))
                compensation.CompensationId = Guid.NewGuid().ToString();

            _employeeContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation GetCompensationByEmployeeId(string employeeId)
        {
            //Gets the most recent compensation
            return _employeeContext.Compensations
                    .Include(c => c.Employee)
                    .Where(c => c.EmployeeId == employeeId)
                    .OrderByDescending(c => c.EffectiveDate) 
                    .FirstOrDefault();
        }
        //Gets Compensation by using compensation id
        public Compensation GetCompensationById(string compensationId)
        {   
            return _employeeContext.Compensations
                .Include(c => c.Employee)
                .FirstOrDefault(c => c.CompensationId == compensationId);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }
    }
}
