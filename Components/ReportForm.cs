using System;
using Crime.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crime.Components
{
  public class ReportForm : ViewComponent
  {
    private ICrimeRepository repository;

    //Konstruktor, som får tillgång till databasen.
    public ReportForm(ICrimeRepository repo)
    {
      repository = repo;
    }

    //Talar om vilken vykomponenet som ska visas.
    public IViewComponentResult Invoke()
    {
      return View("ReportForm");
    }


  }
}
