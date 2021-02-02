using System;
using System.Linq;
using System.Threading.Tasks;
using Crime.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Crime.Components;
using System.Collections.Generic;

namespace Crime.Models
{
  public class EFCrimeRepository : ICrimeRepository
  {
    private ApplicationDbContext context;
    private IHttpContextAccessor contextAcc;

    public EFCrimeRepository(ApplicationDbContext ctx, IHttpContextAccessor cont)
    {
      context = ctx;
      contextAcc = cont;
    }

    //Skapar listor med innehållet från tabellen i databasen.
    public IQueryable<Department> Departments => context.Departments;

    public IQueryable<ErrandStatus> ErrandStatuses => context.ErrandStatuses;

    public IQueryable<Employee> Employees => context.Employees;

    public IQueryable<Errand> Errands => context.Errands.Include(e => e.Samples).Include(e => e.Pictures);

    public IQueryable<Picture> Pictures => context.Pictures;

    public IQueryable<Sample> Samples => context.Samples;

    public IQueryable<Sequence> Sequences => context.Sequences;


    //Hämtar ut det ärende som matchar det inkommande id:et från vykomponenten.
    public Task<Errand> GetErrandInfo(int id)
    {
      return Task.Run(() =>
      {
        var errandInfo = Errands.Where(ei => ei.ErrandId == id).First();
        return errandInfo;
      });
    }

    //Hämtar ut en lista med de anställda som tillhör samma avdelning som användaren loggat in på.
    public IQueryable<Employee> GetEmployee()
    {
      var user = contextAcc.HttpContext.User.Identity.Name;

      var department = context.Employees.Where(em => em.EmployeeId == user).First().DepartmentId;

      var employeeList = Employees.Where(emp => emp.DepartmentId == department);

      return employeeList;
     }

    //Metod som joinar de tabellerna vi behöver information ifrån, för att sedan göra ett nytt MyErrand objekt.
    public IQueryable<MyErrand> GetCoordinatorErrand() {
      var errandList = from err in Errands
                      join stat in ErrandStatuses on err.StatusId equals stat.StatusId
                      join dep in Departments on err.DepartmentId equals dep.DepartmentId into departmentErrand
                      from DeptE in departmentErrand.DefaultIfEmpty()
                      join em in Employees on err.EmployeeId equals em.EmployeeId
                      into employeeErrand
                      from empE in employeeErrand.DefaultIfEmpty()
                      orderby err.RefNumber descending

                      select new MyErrand
                      {
                        DateOfObservation = err.DateOfObservation,
                        ErrandId = err.ErrandId,
                        RefNumber = err.RefNumber,
                        TypeOfCrime = err.TypeOfCrime,
                        StatusName = stat.StatusName,
                        DepartmentName = (err.DepartmentId == null ? "Ej tillsatt" : DeptE.DepartmentName),
                        EmployeeName = (err.EmployeeId == null ? "Ej tillsatt" : empE.EmployeeName)
                      };
      return errandList;
    }

    public IQueryable<MyErrand> GetManagerErrand()
    {
      var user = contextAcc.HttpContext.User.Identity.Name;

      var department = context.Employees.Where(em => em.EmployeeId == user).First().DepartmentId;

      var errandList = from err in Errands
                       join stat in ErrandStatuses on err.StatusId equals stat.StatusId
                       join dep in Departments on err.DepartmentId equals dep.DepartmentId into departmentErrand
                       from DeptE in departmentErrand.DefaultIfEmpty()
                       join em in Employees on err.EmployeeId equals em.EmployeeId
                       into employeeErrand
                       from empE in employeeErrand.DefaultIfEmpty()
                       where err.DepartmentId == department
                       orderby err.RefNumber descending

                       select new MyErrand
                       {
                         DateOfObservation = err.DateOfObservation,
                         ErrandId = err.ErrandId,
                         RefNumber = err.RefNumber,
                         TypeOfCrime = err.TypeOfCrime,
                         StatusName = stat.StatusName,
                         DepartmentName = (err.DepartmentId == null ? "Ej tillsatt" : DeptE.DepartmentName),
                         EmployeeName = (err.EmployeeId == null ? "Ej tillsatt" : empE.EmployeeName)
                       };
      return errandList;
    }

    public IQueryable<MyErrand> GetInvestigatorErrand()
    {
      var user = contextAcc.HttpContext.User.Identity.Name;

      var errandList = from err in Errands
                       join stat in ErrandStatuses on err.StatusId equals stat.StatusId
                       join dep in Departments on err.DepartmentId equals dep.DepartmentId into departmentErrand
                       from DeptE in departmentErrand.DefaultIfEmpty()
                       join em in Employees on err.EmployeeId equals em.EmployeeId
                       into employeeErrand
                       from empE in employeeErrand.DefaultIfEmpty()
                       where err.EmployeeId == user
                       orderby err.RefNumber descending

                       select new MyErrand
                       {
                         DateOfObservation = err.DateOfObservation,
                         ErrandId = err.ErrandId,
                         RefNumber = err.RefNumber,
                         TypeOfCrime = err.TypeOfCrime,
                         StatusName = stat.StatusName,
                         DepartmentName = (err.DepartmentId == null ? "Ej tillsatt" : DeptE.DepartmentName),
                         EmployeeName = (err.EmployeeId == null ? "Ej tillsatt" : empE.EmployeeName)
                       };
      return errandList;
    }

    //Metod som sparar ett nytt ärende med ett nytt ärendenummer som ändras varje gång ett nytt ärende sparas. 
    public string SaveErrand(Errand errand)
    {
      string value = null;

      if (errand.ErrandId == 0)
      {
        var sequence = Sequences.Where(s => s.Id == 1).First();
        int currentValue = sequence.CurrentValue;
        
        value = "2020-45-" + currentValue;

        errand.RefNumber = value;
        errand.StatusId = "S_A";

        context.Errands.Add(errand);

        sequence.CurrentValue++;
      }
      context.SaveChanges();

      return value;
    }

    //Tar in parametrar från controllern. och uppdaterar avdelning hos det inkommande ärendet. 
    public void CoordinatorUpdateDepartment(int errandId, string departmentId)
    {
      Errand dbEntry = context.Errands.FirstOrDefault(e => e.ErrandId == errandId);

        if (dbEntry != null)
      {
        if (departmentId != "D00")
        {
          dbEntry.DepartmentId = departmentId;
        }
      }

      context.SaveChanges();
    }

    //Tar in parametrar från controllern. och uppdaterar handläggare, information och status hos det inkommande ärendet. 
    public void ManagerUpdateInvestigator(int errandId, string employeeId, bool noAction, string reason)
    {
      Errand dbEntry = context.Errands.FirstOrDefault(e => e.ErrandId == errandId);

      if (dbEntry != null)
      {
        dbEntry.EmployeeId = employeeId;
      }

      if (noAction == true)
      {
        dbEntry.InvestigatorInfo = reason;
      }

      if (employeeId == null)
      {
        dbEntry.StatusId = "S_B";
      }

      context.SaveChanges();
    }

    //Tar in parametrar från controllern. och uppdaterar status, lägger till händelser och informatio hos det inkommande ärendet. 
    public void InvestigatorUpdateStatus(int errandId, string events, string information, string statusId)
    {
      Errand dbEntry = context.Errands.FirstOrDefault(e => e.ErrandId == errandId);

      if (dbEntry != null)
      {
        if (statusId != "Välj status")
        {
          dbEntry.StatusId = statusId;
        }

        if (events != null)
        {
          dbEntry.InvestigatorAction += events + " ";
        }

        if (information != null)
        {
          dbEntry.InvestigatorInfo += information + " ";
        }
      }
      context.SaveChanges();
    }

    //Lägger till filnamnet till det prov som tillhör det inkommande ärendet i databasen.
    public void SaveSample(int errandId, string sfileName)
    {
      Sample sample = new Sample
      {
        ErrandId = errandId,
        SampleName = sfileName
      };
      context.Samples.Add(sample);
      context.SaveChanges();
    }

    //Lägger till filnamnet till den bild som tillhör det inkommande ärendet i databasen.
    public void SaveImage(int errandId, string ifileName)
    {
      Picture picture = new Picture
      {
        ErrandId = errandId,
        PictureName = ifileName
      };
      context.Pictures.Add(picture);
      context.SaveChanges();
    }

    



  }
}
