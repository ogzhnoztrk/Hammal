using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Area("Admin")]
public class RoleAdminController : Controller
{
  private readonly RoleManager<IdentityRole> _roleManager;
  private readonly UserManager<IdentityUser> _userManager;

  public RoleAdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
  {
    _roleManager = roleManager;
    _userManager = userManager;
  }

  public IActionResult Index()
  {
    var roles = _roleManager.Roles.ToList();
    return View("Index",roles);
  }

  public IActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Create(string roleName)
  {
    if (ModelState.IsValid)
    {
      IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(roleName));
      if (result.Succeeded)
      {
        return RedirectToAction("Index", "RoleAdmin");
      }
      else
      {
        foreach (var error in result.Errors)
        {
          ModelState.AddModelError("", error.Description);
        }
      }
    }
    return View(roleName);
  }
}
