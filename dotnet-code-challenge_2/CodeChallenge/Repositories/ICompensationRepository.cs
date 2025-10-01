using CodeChallenge.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation Add(Compensation compensation); //Adds new compensation obj to db 
        Compensation GetCompensationByEmployeeId(string employeeId);
        Compensation GetCompensationById(string compensationId);
        Task SaveAsync();
        

    }
}