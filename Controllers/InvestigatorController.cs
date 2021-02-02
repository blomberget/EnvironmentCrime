using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Crime.Models;
using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crime.Controllers
{  [Authorize(Roles = "Investigator")]
  public class InvestigatorController : Controller
  {
    private ICrimeRepository repository;

    private IWebHostEnvironment environment;

    //Konstruktor, som får tillgång till databasen och webbmiljön.
    public InvestigatorController(ICrimeRepository repo, IWebHostEnvironment env)
    {
      repository = repo;
      environment = env;
    }

    public ViewResult StartInvestigator()
    {
      ViewBag.Title = "Start - Utredare";
      //Returnerar vyn som har tillgång och kan representera data från databasen.
      return View(repository);
    }

    public ViewResult CrimeInvestigator(int id)
    {
      ViewBag.Title = "Handläggare";

      //Ta in ett id och skicka vidare detta till View som anropar vykomponenten, som tar in id
      ViewBag.ID = id;
      //Spara id temporärt inom controllern, så andra controller-metoder kan använda det.
      TempData["ID"] = id;
      //Anropa metoden GetEmployee i repository, och skicka resultatet till vyn som använder viewbag.
      ViewBag.ListofStatuses = repository.ErrandStatuses;
      return View();
    }

    public async Task<IActionResult> InvestigatorUpdateStatus(string events, string information, string statusId, IFormFile loadSample, IFormFile loadImage)
    {
      ViewBag.Title = "Status uppdaterad";

      //Tom behållare för filnamnet.
      string sfileName = "";
      string ifileName = "";

      //Den temporära data som hanteras inom kontrollern, görs om till en string och sparas i en variabel. 
      int errandId = int.Parse(TempData["ID"].ToString());

      //Om det finns ett prov uppladdat, spara detta till en temporär fil.
      if (loadSample != null)
      {
        var tempSPath = Path.GetTempFileName();

        if (loadSample.Length > 0)
        {
          using (var stream = new FileStream(tempSPath, FileMode.Create))
          {
            await loadSample.CopyToAsync(stream);
          }
        }
        //Skapa ny sökväg
        var samplePath = Path.Combine(environment.WebRootPath, "SampleUploads", loadSample.FileName);

        //Flytta temporär fil till den nya sökvägen.
        System.IO.File.Move(tempSPath, samplePath);

        //Uppdatera behållaren för filnamnet.
        sfileName = loadSample.FileName;

        //Anropa metoden i repository som sparar filnamnet till databasen.
        repository.SaveSample(errandId, sfileName);
      }

      //Om det finns en bild uppladdat, spara detta till en temporär fil.
      if (loadImage != null)
      {
        var tempIPath = Path.GetTempFileName();

        if (loadImage.Length > 0)
        {
          using (var stream = new FileStream(tempIPath, FileMode.Create))
          {
            await loadImage.CopyToAsync(stream);
          }
        }
        //Skapa ny sökväg
        var IsamplePath = Path.Combine(environment.WebRootPath, "ImageUploads", loadImage.FileName);

        //Flytta temporär fil till den nya sökvägen.
        System.IO.File.Move(tempIPath, IsamplePath);

        //Uppdatera behållaren för filnamnet.
        ifileName = loadImage.FileName;

        //Anropa metoden i repository som sparar filnamnet till databasen.
        repository.SaveImage(errandId, ifileName);
      }

      //Anropa metoden InvestigatorUpdateStatus som finns i repository och skicka med variabler. 
      repository.InvestigatorUpdateStatus(errandId, events, information, statusId);

      //Skicka tillbaka id till CrimeInvestigator Action
      return RedirectToAction("CrimeInvestigator", new { id = errandId });
    }


  }
}
