using Hammal.DataAccess.Repository.IRepository;
using Hammal.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using NuGet.Protocol.Plugins;

namespace HammalWeb.Areas.Customer.Controllers
{
  [Area("Customer")]
  public class CategoryController : Controller
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    public CategoryController(IUnitOfWork unitOfWork, IEmailSender emailSender)
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
    public IActionResult CategoryDetail(int? Id)
    {


      if (Id == 0 || Id == null)
      {
        return View("CategoryDetail");

      }
      else
      {
        var altCategoryFromDb = _unitOfWork.AltCategory.GetAll().Where(x => x.CategoryId == Id).ToList();


        return View("CategoryDetail", altCategoryFromDb);
      }


    }

    [HttpGet]
    public JsonResult GetAllAltCategories()
    {
      var altCategoryList = _unitOfWork.AltCategory.GetAll();
      return Json(new { data = altCategoryList });

    }
    [HttpGet]
    public JsonResult GetAltCategoryById(int id)
    {
      var altCategoryList = _unitOfWork.AltCategory.GetAll().Where(x => x.CategoryId == id);
      return Json(new { data = altCategoryList });

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
