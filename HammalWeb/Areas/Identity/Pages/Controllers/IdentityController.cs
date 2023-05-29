using Hammal.DataAccess.Repository;
using Hammal.DataAccess.Repository.IRepository;
using Hammal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Area("Identity")]
public class IdentityController : Controller
{
  private readonly RoleManager<IdentityRole> _roleManager;
  private readonly UserManager<IdentityUser> _userManager;
  private readonly IUnitOfWork _unitOfWork;

  public IdentityController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager,IUnitOfWork unitOfWork)
  {
    _roleManager = roleManager;
    _userManager = userManager;
    _unitOfWork = unitOfWork;
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

  public IActionResult GetDistricts(string id)
  {
    var a = Int32.Parse(id);

    var districts = _unitOfWork.District.GetAll(i => i.CityId == a)
        .Select(d => new SelectListItem
        {
          Text = d.Name,
          Value = d.Id.ToString()
        })
        .ToList();

    return Json(districts);

  }




}
