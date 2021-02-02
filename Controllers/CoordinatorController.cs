using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crime.Models;
using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SessionTest.Infrastructure;
using Crime.Components;

namespace Crime.Controllers
{  [Authorize(Roles = "Coordinator")]  public class CoordinatorController : Controller
  {
    private ICrimeRepository repository;

    //Konstruktor, som får tillgång till databasen.
    public CoordinatorController(ICrimeRepository repo)
    {
      repository = repo;
    }    public ViewResult StartCoordinator()
    {
      ViewBag.Title = "Start - Samordnare";
      //Returnerar vyn som har tillgång och kan representera data från databasen.
      return View(repository);
    }        public ViewResult ReportCrime()
    {
      ViewBag.Title = "Rapportera brott";

      //Sparar det inkommande ärendet från formuläret till en session om det finns något inskickat. Sparar informationen i en session till den skickas in.
      var newErrand = HttpContext.Session.GetJson<Errand>("NewErrand");
      if (newErrand == null)
      {
        return View();
      }
      else
      {
        return View(newErrand);
      }
    }

    [HttpPost]
    public ViewResult Validate(Errand errand)
    {
      //Håller i sessionen om ärendet
      HttpContext.Session.SetJson("NewErrand", errand);
      return View(errand);
    }


    public ViewResult Thanks()
    {
      ViewBag.Title = "Tack!";
      //Hämta ut från sessionen för att spara i databasen
      var newErrand = HttpContext.Session.GetJson<Errand>("NewErrand");

      string refnumber = repository.SaveErrand(newErrand);

      ViewBag.refnumber = refnumber;

      HttpContext.Session.Remove("NewErrand");
      return View();
    }

    public ViewResult CrimeCoordinator(int id)
    {
      ViewBag.Title = "Samordnare";

      //Ta in ett id och skicka vidare detta till View som anropar vykomponenten, som tar in id
      ViewBag.ID = id;
      //Spara id temporärt inom controllern, så andra controller-metoder kan använda det.
      TempData["ID"] = id;
      //Anropa metoden GetEmployee i repository, och skicka resultatet till vyn som använder viewbag.
      ViewBag.ListofDepartments = repository.Departments;
      return View();
    }

    public IActionResult CoordinatorUpdateDepartment(string departmentId)
    {
      ViewBag.Title = "Avdelning uppdaterad";

      //Den temporära data som hanteras inom kontrollern, görs om till en string och sparas i en variabel.
      int errandId = int.Parse(TempData["ID"].ToString());

      //Anropa metoden CoordinatorUpdateDepartment som finns i repository och skicka med variabler. 
      repository.CoordinatorUpdateDepartment(errandId, departmentId);

      //Skicka tillbaka id till CrimeCoordinator Action
      return RedirectToAction("CrimeCoordinator", new { id = errandId });
    }

  }
}
