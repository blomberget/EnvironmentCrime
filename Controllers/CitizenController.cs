using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Crime.Models;
using SessionTest.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace Crime.Controllers
{  [AllowAnonymous]
  public class CitizenController : Controller
  {
    private ICrimeRepository repository;

    public CitizenController(ICrimeRepository repo)
    {
      repository = repo;
    }

    public ViewResult Contact()
    {
      ViewBag.Title = "Kontakt";
      return View();
    }

    public ViewResult FAQ()
    {
      ViewBag.Title = "FAQ";
      return View();
    }

    public ViewResult Services()
    {
      ViewBag.Title = "Tjänster";
      return View();
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

  }
}
