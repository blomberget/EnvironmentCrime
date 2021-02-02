using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crime.Models;
using Microsoft.AspNetCore.Http;

//Möjliggör användandet av metoder mellan controller och repository.
namespace Crime.Models
{
  public interface ICrimeRepository
  {
    IQueryable<Department> Departments { get; }
    IQueryable<Employee> Employees { get; }
    IQueryable<Errand> Errands { get; }
    IQueryable<ErrandStatus> ErrandStatuses { get; }
    IQueryable<Picture> Pictures { get; }
    IQueryable<Sample> Samples { get; }
    IQueryable<Sequence> Sequences{ get; }

    IQueryable<MyErrand> GetCoordinatorErrand();
    IQueryable<MyErrand> GetManagerErrand();
    IQueryable<MyErrand> GetInvestigatorErrand();
    IQueryable<Employee> GetEmployee();

    Task<Errand> GetErrandInfo(int id);

    string SaveErrand(Errand errand);

    void SaveSample(int errandId, string sfileName);
    void SaveImage(int errandId, string sfileName);
    void CoordinatorUpdateDepartment(int errandId, string departmentId);
    void ManagerUpdateInvestigator(int errandId, string employeeId, bool noAction, string reason);
    void InvestigatorUpdateStatus(int errandId, string events, string information, string statusId);
  }
}
