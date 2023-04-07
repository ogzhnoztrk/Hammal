using Hammal.DataAccess.Repository.IRepository;
using Hammal.Models;
using Hammal.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace HammalWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AdvertisementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdvertisementController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {

            return View();
        }

        //GET
        public IActionResult Upsert(int? id)
        {

            AdvertisementVM advertisementVM = new()
            {
                Advertisement = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };



            if (id == 0 || id == null)
            {
                return View(advertisementVM);

            }
            else
            {
                advertisementVM.Advertisement = _unitOfWork.Advertisement.GetFirstOrDefault(i => i.Id == id);

                return View(advertisementVM);
            }


        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(AdvertisementVM obj)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


			if (User.Identity.IsAuthenticated)
			{
				 if (ModelState.IsValid)
                 {
                     if (obj.Advertisement.Id==0 || obj.Advertisement==null)
                     {
                         obj.Advertisement.AdvertiserID = claim.Value;
                         _unitOfWork.Advertisement.Add(obj.Advertisement); //Veri tabanına kayıt
                         TempData["success"] = "İlan Eklendi";
                        return RedirectToAction("Index");
                     }
                     else
                     {
                         _unitOfWork.Advertisement.Update(obj.Advertisement);
                         TempData["success"] = "İlan Güncellendi";
                          
                     }
                     _unitOfWork.Save();
                 }
            }
            return View(obj);
        }



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var advertisementList = _unitOfWork.Advertisement.GetAll(includeProperties:"Category");
            return Json(new { data = advertisementList });

        }


        [HttpDelete]

        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Advertisement.GetFirstOrDefault(c => c.Id == id);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }



            _unitOfWork.Advertisement.Remove(obj);
            _unitOfWork.Save(); TempData["success"] = "Advertisement deleted successflly";

            return Json(new { success = true, message = "Delete successful." });


        }


        #endregion

    }
}
