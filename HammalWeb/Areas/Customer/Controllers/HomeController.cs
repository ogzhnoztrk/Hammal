using AutoMapper;
using Hammal.DataAccess.Repository.IRepository;
using Hammal.Models;
using Hammal.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
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
            var categories= _unitOfWork.Category.GetAll().ToList();
            return View("Index",categories);
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