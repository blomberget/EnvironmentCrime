using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Crime.Models;
using System.Threading.Tasks;

namespace Crime.Components
{
  public class ErrandInfo : ViewComponent
  {
    private ICrimeRepository repository;

    //Konstruktor, som får tillgång till databasen.
    public ErrandInfo(ICrimeRepository repo)
    {
      repository = repo;
    }

    //Returnerar vy-resultat. Returnerar async task. 
    public async Task<IViewComponentResult> InvokeAsync(int id)
    {
      //Model
      var errandInfo = await repository.GetErrandInfo(id);
      return View(errandInfo);
    }

  }
}
