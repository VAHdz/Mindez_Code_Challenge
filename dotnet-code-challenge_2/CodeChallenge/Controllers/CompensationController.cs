using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger<CompensationController> _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpGet("{employeeId}")]
        public IActionResult GetCompensationByEmployeeId(string employeeId)
        {
            if (string.IsNullOrEmpty(employeeId))
                return BadRequest("employeeId not provided");


            var compensation = _compensationService.GetCompensationByEmployeeId(employeeId);

            if (compensation == null){return NotFound();} //Error Check: If no compensation found for specific employee

            return Ok(compensation);
        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            if (compensation == null || string.IsNullOrEmpty(compensation.EmployeeId))
            {
                return BadRequest("Compensatio/ EmployeeId is null/empty"); //Use these comments for compensation testing 
            }

            _logger.LogDebug($"Received create request for EmployeeId: '{compensation.EmployeeId}', Salary: {compensation.Salary}");

            var createdCompensation = _compensationService.Create(compensation); //Compensation object created 

            return CreatedAtAction(nameof(GetCompensationByEmployeeId),
                    new { employeeId = createdCompensation.EmployeeId },
                    createdCompensation);
        }

       

    }
}
