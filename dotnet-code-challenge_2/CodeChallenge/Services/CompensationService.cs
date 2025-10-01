using System;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services

{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
       
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
        {
            _compensationRepository = compensationRepository;
            _logger = logger;
            
        }

        public Compensation Create(Compensation compensation)
        {
            //Error Check: check for null value 
            if (compensation == null)
                throw new ArgumentNullException(nameof(compensation));

            //Error Check: employee id provided
            if (string.IsNullOrEmpty(compensation.EmployeeId))
                throw new ArgumentException("no EmployeeId provided ", nameof(compensation.EmployeeId));
            
               


            //Error Check: Assign a CompensationId if not set
            if (string.IsNullOrEmpty(compensation.CompensationId))
                compensation.CompensationId = Guid.NewGuid().ToString();

            _compensationRepository.Add(compensation);
            _compensationRepository.SaveAsync().Wait(); // blocking call

            _logger.LogInformation($"Created compensation for EmployeeId: {compensation.EmployeeId}");

            return compensation;
        }

        public Compensation GetCompensationByEmployeeId(string employeeId)
        {
            if (string.IsNullOrEmpty(employeeId)) //Error Check: No EmployeeId provided 
                return null;

            var compensation = _compensationRepository.GetCompensationByEmployeeId(employeeId);

            if (compensation == null)
                _logger.LogWarning($"No compensation found for EmployeeId: {employeeId}");

            return compensation;
        }

        public Compensation GetCompensationById(string compensationId)
        {
            if (string.IsNullOrEmpty(compensationId))//Error Check: No compensatioId provided 
                return null;

            var compensation = _compensationRepository.GetCompensationById(compensationId);

            if (compensation == null)
                _logger.LogWarning($"No compensation found with CompensationId: {compensationId}");

            return compensation;
        }
    }
}
