using Hammal.DataAccess.Repository;
using Hammal.DataAccess.Repository.IRepository;
using Hammal.Models;
using Hammal.Models.Dtos;
using Hammal.Models.ViewModels;
using Hammal.Utilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace HammalWeb.Areas.Customer.Controllers
{
	[Area("Customer")]

	public class ServiceController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IEmailSender _emailSender;
		private readonly IRepository<SystemUser> _repo;
		private readonly IRepository<Address> _adressRepo;
		private readonly IRepository<City> _cityRepo;
		private readonly IRepository<District> _districtRepo;
		private readonly IRepository<Category> _categoryRepo;
		private readonly IRepository<AltCategory> _altCategoryRepo;

		public ServiceController(IUnitOfWork unitOfWork, IEmailSender emailSender)
		{

			_unitOfWork = unitOfWork;
			_emailSender = emailSender;
			_repo = UnitOfWork.GetRepository<SystemUser>();
			_adressRepo = UnitOfWork.GetRepository<Address>();
			_cityRepo = UnitOfWork.GetRepository<City>();
			_districtRepo = UnitOfWork.GetRepository<District>();
			_categoryRepo = UnitOfWork.GetRepository<Category>();
			_altCategoryRepo = UnitOfWork.GetRepository<AltCategory>();
		}
		public IActionResult Index()
		{
			return View();
		}

		//GET
		public IActionResult Upsert(int? id)
		{


			if (id == 0 || id == null)
			{
				return View();

			}
			else
			{
				var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);

				return View(categoryFromDb);
			}


		}

		//GET
		public IActionResult CategoryDetail(int? id)
		{


			if (id == 0 || id == null)
			{
				return View("CategoryDetail");

			}
			else
			{
				var altCategoryFromDb = _unitOfWork.AltCategory.GetAll().Where(x => x.CategoryId == id).ToList();


				return View("CategoryDetail", altCategoryFromDb);
			}


		}


		//GET
		public IActionResult EmployeeDetailView(int? id)
		{


			if (id == 0 || id == null)
			{
				return View("EmployeeForm");

			}
			else
			{
				var altCategoryFromDb = _unitOfWork.AltCategory.GetAll().Where(x => x.CategoryId == id).ToList();


				return View("CategoryDetail", altCategoryFromDb);
			}


		}

		public async Task<IActionResult> ServiceClaimView(int? altCategoryId, int? cityId, int? districtId, double? priceMax, double? priceMin)
		{
			var modelList = new List<SystemUserDto>();
			var systemUserList = new List<SystemUser>();
			if (altCategoryId == null)
			{
				systemUserList = await _repo.GetAll().Include(x => x.ApplicationUser).ToListAsync();

			}
			else
			{
				systemUserList = await _repo.Find(x => x.AltCategoryId == altCategoryId).Include(x => x.ApplicationUser).ToListAsync();
			}	

			foreach (var systemUser in systemUserList)
			{
				var model = new SystemUserDto();

				var address = await _adressRepo.Find(x => x.ApplicationUserId == systemUser.ApplicationUserId).FirstOrDefaultAsync();
				var district = await _districtRepo.Find(x => x.Id == address.DistrictId).FirstOrDefaultAsync();
				var city = await _cityRepo.Find(x => x.Id == district.CityId).FirstOrDefaultAsync();


				model.Id = systemUser.Id;
				model.Name = systemUser.ApplicationUser.Name;
				model.Email = systemUser.ApplicationUser.Email;
				model.Price = systemUser.Price;
				model.Abilities = systemUser.Abilities;
				model.CityName = city.Name;
				model.CityId = city.Id;
				model.DistrictName = district.Name;
				model.DistrictId = district.Id;
				model.AltCategoryName = await _altCategoryRepo.Find(x => x.Id == systemUser.AltCategoryId).Select(y => y.Name).FirstOrDefaultAsync();
				model.CategoryName = await _altCategoryRepo.Find(x => x.Id == systemUser.CategoryId).Select(y => y.Name).FirstOrDefaultAsync();
				modelList.Add(model);
			}

			if (cityId != null)
			{
				modelList = modelList.Where(x => x.CityId == cityId).ToList();
			}
			else if (districtId != null)
			{
				modelList = modelList.Where(x => x.DistrictId == districtId).ToList();
			}
			else if (priceMax != null || priceMin != null)
			{
				modelList = modelList.Where(x => x.Price <= priceMax && x.Price >= priceMin).ToList();
			}

			var cities = await _cityRepo.GetAll().ToListAsync(); ;
			ViewBag.Cities = cities;
			ViewBag.Districts = "";
			ViewBag.AltCategoryId = altCategoryId ?? -1;

			return View("ServiceClaim", modelList);
		}

		public async Task<IActionResult> OnChangeCities(int? cityId)
		{
			var cities = await _repo.GetAll().ToListAsync();
			ViewBag.Cities = cities;
			var districts = await _districtRepo.Find(x => x.CityId == cityId).ToListAsync();
			ViewBag.Districts = districts;
			return Json(new { success = true, districts = districts });

		}



		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(Category category)
		{
			var categoriesFromDb = _unitOfWork.Category.GetFirstOrDefault(filter => filter.Name == category.Name);
			if (ModelState.IsValid)
			{
				if (category.Id == 0)
				{
					if (categoriesFromDb == null)
					{
						_unitOfWork.Category.Add(category);
						TempData["success"] = "Kategori Oluşturuldu";
						_unitOfWork.Save();
						return RedirectToAction("Index");
					}
					else
					{
						TempData["error"] = "Kategori Mevcut";
						return RedirectToAction("Index");
					}
				}
				else
				{
					if (categoriesFromDb == null)
					{
						_unitOfWork.Category.Update(category);
						TempData["success"] = "Kategori Güncellendi";
						_unitOfWork.Save();
						return RedirectToAction("Index");
					}
					else
					{
						TempData["error"] = "Kategori Mevcut";
						return RedirectToAction("Index");
					}
				}
			}
			return View(category);
		}

		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpsertSystemUser(SystemUser systemUser)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			//claim.Value mevcut giriş yapan kullanıcının idsini veriyor

			// var existingRecord = _unitOfWork.SystemUser.GetFirstOrDefault(x => x.ApplicationUserId == claim.Value);
			var existingRecord = await _repo.FirstOrDefaultAsync(x => x.ApplicationUserId == claim.Value);
			if (ModelState.IsValid)
			{

				if (existingRecord == null)
				{
					systemUser.ApplicationUserId = claim.Value;

					_unitOfWork.SystemUser.Add(systemUser);
					await _unitOfWork.SaveAsync();
					return RedirectToAction("Index", "Service");
				}
				else
				{
					existingRecord.CategoryId = systemUser.CategoryId;
					existingRecord.AltCategoryId = systemUser.AltCategoryId;
					existingRecord.Price = systemUser.Price;
					existingRecord.Abilities = systemUser.Abilities;
					_repo.Update(existingRecord);
					await _unitOfWork.SaveAsync();
					return RedirectToAction("Index", "Service");
				}

			}
			return View("Index");
		}

		[HttpGet]
		public IActionResult OrderCard(int systemUserId)
		{

			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			if (claim !=null)
			{
				ShoppingCart shoppingCart = new()
				{
					ApplicationUserId = claim.Value,
					ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(i => i.Id == claim.Value),
					SystemUserId = systemUserId,
					SystemUser = _unitOfWork.SystemUser.GetFirstOrDefault(i => i.Id == systemUserId),

				};



				var shoppingCartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
				  i => i.ApplicationUserId == claim.Value && i.SystemUserId == shoppingCart.SystemUserId);

				if (shoppingCartFromDb == null)
				{

					shoppingCart.OrderTotal = (double)shoppingCart.SystemUser.Price;
					_unitOfWork.ShoppingCart.Add(shoppingCart);
					_unitOfWork.Save();
				}
				else
				{
					if (shoppingCartFromDb.SystemUserId !=systemUserId)
					{
						shoppingCart.OrderTotal = shoppingCart.OrderTotal + (double)shoppingCart.SystemUser.Price;
						_unitOfWork.ShoppingCart.Update(shoppingCart);
						_unitOfWork.Save();
					}
					else
					{
						return RedirectToAction("OrderCard", "Order");

					}


				}
				return RedirectToAction("OrderCard", "Order");

			}
			else
			{
				//Burada giriş yapılması gerektiğine dair bir uyari firletacak
				return View();
			}

		}


		#region API CALLS
		[HttpGet]
		public IActionResult GetAll()
		{
			var categoryList = _unitOfWork.Category.GetAll();
			return Json(new { data = categoryList });

		}


		[HttpDelete]

		public IActionResult Delete(int? id)
		{
			var obj = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);

			if (obj == null)
			{
				return Json(new { success = false, message = "Error while deleting." });
			}



			_unitOfWork.Category.Remove(obj);
			_unitOfWork.Save(); TempData["success"] = "Product deleted successflly";

			return Json(new { success = true, message = "Delete successful." });


		}


		#endregion

	}
}
