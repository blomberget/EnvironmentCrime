using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crime.Models;
using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;


namespace Crime.Controllers
{  [Authorize(Roles = "Manager")]
  public class ManagerController : Controller
  {
    private ICrimeRepository repository;

    //Konstruktor, som får tillgång till databasen.
    public ManagerController(ICrimeRepository repo)
    {
      repository = repo;
    }

    public ViewResult StartManager()
    {
      ViewBag.Title = "Start - Chef";
      //Returnerar vyn som har tillgång och kan representera data från databasen.
      return View(repository);
    }

    public ViewResult CrimeManager(int id)
    {
      ViewBag.Title = "Chef";

      //Ta in ett id och skicka vidare detta till View som anropar vykomponenten, som tar in id
      ViewBag.ID = id;
      //Spara id temporärt inom controllern, så andra controller-metoder kan använda det.
      TempData["ID"] = id;
      //Anropa metoden GetEmployee i repository, och skicka resultatet till vyn som använder viewbag.
      ViewBag.ListofEmployees = repository.GetEmployee();
      return View();
    }

    public IActionResult ManagerUpdateInvestigator(string employeeId, bool noAction, string reason)
    {
      ViewBag.Title = "Handläggare uppdaterad";

      //Om knappen "Ingen åtgärd" inte är iklickad, spara inte anledning. 
      if (noAction != true) {
        reason = null;
      }

      //Om knappen "Ingen åtgärd" är iklickad, spara anledning, och tilldela ingen handläggare på ärendet. 
      else
      {
        employeeId = null;
      }

      //Den temporära data som hanteras inom kontrollern, görs om till en string och sparas i en variabel. 
      int errandId = int.Parse(TempData["ID"].ToString());

      //Anropa metoden ManagerUpdateinvestigator som finns i repository och skicka med variabler. 
      repository.ManagerUpdateInvestigator(errandId, employeeId, noAction, reason);

      //Skicka tillbaka id till CrimeManager Action
      return RedirectToAction("CrimeManager", new { id = errandId });
    }


  }
}
