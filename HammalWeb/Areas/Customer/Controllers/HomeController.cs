using AutoMapper;
using Hammal.DataAccess.Repository.IRepository;
using Hammal.Models;
using Hammal.Models.Dtos;
using HammalWeb.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace HammalWeb.Areas.Customer.Controllers
{
  [Area("Customer")]
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IMapper mapper)
    {
      _logger = logger;
      _unitOfWork = unitOfWork;
      _mapper = mapper;
     
    }

    public IActionResult Index()
    {
      var advertisements = _unitOfWork.Advertisement.GetAll();
      var categories = _unitOfWork.Category.GetAll().ToList();
      return View("Index", categories);
    }

    [HttpGet]
    public async Task<IEnumerable<SelectListItem>> GetDistricts(int id)
    {

      
      var DistrictList = _unitOfWork.District.GetAll(i => i.CityId == id).Select(i => new SelectListItem
      {
        Text = i.Name,
        Value = i.Id.ToString()
      });


      return DistrictList;

    }


    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}