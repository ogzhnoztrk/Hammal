using Hammal.DataAccess.Repository;
using Hammal.DataAccess.Repository.IRepository;
using Hammal.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace HammalWeb.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;

        public ServiceController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {

            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
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

      var existingRecord = _unitOfWork.SystemUser.GetFirstOrDefault(x => x.ApplicationUserId== claim.Value);
      if (ModelState.IsValid)
      {
      
          if (existingRecord == null)
          {
            _unitOfWork.SystemUser.Add(systemUser);
            TempData["success"] = "Kategori Oluşturuldu";
            _unitOfWork.Save();
            return RedirectToAction("Index");
          }
          else
          {
            existingRecord.Id = systemUser.Id;
            existingRecord.CategoryId = systemUser.CategoryId;
            existingRecord.AltCategoryId = systemUser.AltCategoryId;
            _unitOfWork.SystemUser.Update(existingRecord);
            TempData["success"] = "Kategori Güncellendi";
            _unitOfWork.Save();
            return RedirectToAction("Index");
          }
        
      }
      return View("Index");
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
