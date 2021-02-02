using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Crime.Models;
using SessionTest.Infrastructure;using Microsoft.AspNetCore.Http;

namespace Crime.Controllers
{  [Authorize(Roles="Coordinator, Investigator, Manager")]  public class HomeController : Controller
  {
    private UserManager<IdentityUser> userManager;
    private SignInManager<IdentityUser> signInManager;

    public HomeController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr)
    {
      userManager = userMgr;
      signInManager = signInMgr;
    }


    [AllowAnonymous]    public ViewResult Index()
    {
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

    //Hämtar information om den inloggade användaren och skickar denne till rätt sida.
    [AllowAnonymous]
    public ViewResult Login(string returnUrl)
    {
      ViewBag.Title = "Login";
      return View(new LoginModel
      {
        ReturnUrl = returnUrl,
      });
    }

    //Kollar inloggningsuppgifter och ser ifall det finns en användare som matchar dessa. Därefter koppla ihop användarroll med rätt sida denne ska skickas till.
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
      IdentityUser user = await userManager.FindByNameAsync(loginModel.UserName);

      if (ModelState.IsValid)      {
        if (user != null)
        {
          await signInManager.SignOutAsync();
          if ((await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
          {
            if (await userManager.IsInRoleAsync(user, "Coordinator"))
            {
              return Redirect("/Coordinator/StartCoordinator");
            }

            if (await userManager.IsInRoleAsync(user, "Investigator"))
            {              return Redirect("/Investigator/StartInvestigator");
            }

            if (await userManager.IsInRoleAsync(user, "Manager"))
            {              return Redirect("/Manager/StartManager"); 
            }
          }
        }
      }
      ModelState.AddModelError("", "Felaktigt användarnamn eller lösenord.");
      return View(loginModel);
    }
    //Loggar ut användare
        public async Task<RedirectResult> Logout(string returnUrl = "/Home/Login")
    {
      await signInManager.SignOutAsync();
      return Redirect(returnUrl);
    }

    //Åtkomst nekad
    [AllowAnonymous]
    public ViewResult AccessDenied()
    {
      return View();
    }
  }
}
