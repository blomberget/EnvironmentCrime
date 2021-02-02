using System;using System.Threading.Tasks;using Microsoft.AspNetCore.Identity;using Microsoft.Extensions.DependencyInjection;namespace Crime.Models{  public class IdentityInitializer  {    public static async Task EnsurePopulated(IServiceProvider services)    {      var userManager = services.GetRequiredService<UserManager<IdentityUser>>();      var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();      await CreateRoles(roleManager);      await CreateUsers(userManager);    }    //Skapa olika roller som användare kan ha.    private static async Task CreateRoles(RoleManager<IdentityRole> rManager)    {      if (!await rManager.RoleExistsAsync("Coordinator"))      {        await rManager.CreateAsync(new IdentityRole("Coordinator"));      }      if (!await rManager.RoleExistsAsync("Investigator"))      {        await rManager.CreateAsync(new IdentityRole("Investigator"));      }      if (!await rManager.RoleExistsAsync("Manager"))      {        await rManager.CreateAsync(new IdentityRole("Manager"));      }    }    //Skapar inloggningsuppgifter och lägger till roll till användare.    private static async Task CreateUsers(UserManager<IdentityUser> uManager)    {
      IdentityUser E001 = new IdentityUser("E001");
      await uManager.CreateAsync(E001, "Pass01?");      await uManager.AddToRoleAsync(E001, "Coordinator");      IdentityUser E100 = new IdentityUser("E100");
      await uManager.CreateAsync(E100, "Pass02?");
      await uManager.AddToRoleAsync(E100, "Manager");

      IdentityUser E101 = new IdentityUser("E101");
      await uManager.CreateAsync(E101, "Pass03?");
      await uManager.AddToRoleAsync(E101, "Investigator");

      IdentityUser E102 = new IdentityUser("E102");
      await uManager.CreateAsync(E102, "Pass04?");
      await uManager.AddToRoleAsync(E102, "Investigator");

      IdentityUser E103 = new IdentityUser("E103");
      await uManager.CreateAsync(E103, "Pass05?");
      await uManager.AddToRoleAsync(E103, "Investigator");

      IdentityUser E200 = new IdentityUser("E200");
      await uManager.CreateAsync(E200, "Pass06?");
      await uManager.AddToRoleAsync(E200, "Manager");

      IdentityUser E201 = new IdentityUser("E201");
      await uManager.CreateAsync(E201, "Pass07?");
      await uManager.AddToRoleAsync(E201, "Investigator");

      IdentityUser E202 = new IdentityUser("E202");
      await uManager.CreateAsync(E202, "Pass08?");
      await uManager.AddToRoleAsync(E202, "Investigator");

      IdentityUser E203 = new IdentityUser("E203");
      await uManager.CreateAsync(E203, "Pass09?");
      await uManager.AddToRoleAsync(E203, "Investigator");

      IdentityUser E300 = new IdentityUser("E300");
      await uManager.CreateAsync(E300, "Pass10?");
      await uManager.AddToRoleAsync(E300, "Manager");

      IdentityUser E301 = new IdentityUser("E301");
      await uManager.CreateAsync(E301, "Pass11?");
      await uManager.AddToRoleAsync(E301, "Investigator");

      IdentityUser E302 = new IdentityUser("E302");
      await uManager.CreateAsync(E302, "Pass12?");
      await uManager.AddToRoleAsync(E302, "Investigator");

      IdentityUser E303 = new IdentityUser("E303");
      await uManager.CreateAsync(E303, "Pass13?");
      await uManager.AddToRoleAsync(E303, "Investigator");

      IdentityUser E400 = new IdentityUser("E400");
      await uManager.CreateAsync(E400, "Pass14?");
      await uManager.AddToRoleAsync(E400, "Manager");

      IdentityUser E401 = new IdentityUser("E401");
      await uManager.CreateAsync(E401, "Pass15?");
      await uManager.AddToRoleAsync(E401, "Investigator");

      IdentityUser E402 = new IdentityUser("E402");
      await uManager.CreateAsync(E402, "Pass16?");
      await uManager.AddToRoleAsync(E402, "Investigator");

      IdentityUser E403 = new IdentityUser("E403");
      await uManager.CreateAsync(E403, "Pass17?");
      await uManager.AddToRoleAsync(E403, "Investigator");

      IdentityUser E500 = new IdentityUser("E500");
      await uManager.CreateAsync(E500, "Pass18?");
      await uManager.AddToRoleAsync(E500, "Manager");

      IdentityUser E501 = new IdentityUser("E501");
      await uManager.CreateAsync(E501, "Pass19?");
      await uManager.AddToRoleAsync(E501, "Investigator");

      IdentityUser E502 = new IdentityUser("E502");
      await uManager.CreateAsync(E502, "Pass20?");
      await uManager.AddToRoleAsync(E502, "Investigator");

      IdentityUser E503 = new IdentityUser("E503");
      await uManager.CreateAsync(E503, "Pass21?");
      await uManager.AddToRoleAsync(E503, "Investigator");

    }  }}