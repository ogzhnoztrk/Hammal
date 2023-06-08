using Hammal.DataAccess.Repository.IRepository;
using Hammal.Models;
using Hammal.Models.ViewModels;
using Hammal.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HammalWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
		[BindProperty]
		public ShoppingCartVM shoppingCartVM { get; set; }
		private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        //HTTPGET
        public IActionResult OrderCard()
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCartVM = new()
            {
                CartList = _unitOfWork.ShoppingCart.Find(i=>i.ApplicationUserId == claim.Value).AsQueryable().Include(i=>i.SystemUser).ThenInclude(i=>i.AltCategory),
                Order = new()
            };
			shoppingCartVM.Order.Address = _unitOfWork.Address.Find(x => x.ApplicationUserId == claim.Value).AsQueryable().Include(i => i.District).ThenInclude(i => i.City).FirstOrDefault();
			foreach (var item in shoppingCartVM.CartList)
            {
                shoppingCartVM.Order.OrderTotal = shoppingCartVM.Order.OrderTotal + (double)item.SystemUser.Price; 
            }
			return View(shoppingCartVM);

        }

        public IActionResult Summary()
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			shoppingCartVM = new ShoppingCartVM()
			{
				CartList = _unitOfWork.ShoppingCart.GetAll(i => i.ApplicationUserId == claim.Value,includeProperties:"SystemUser,ApplicationUser"),
				Order = new()
			};

			shoppingCartVM.Order.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(
				u => u.Id == claim.Value);
            shoppingCartVM.Order.CustomerId = claim.Value;
            shoppingCartVM.Order.Address = _unitOfWork.Address.Find(x => x.ApplicationUserId == claim.Value).AsQueryable().Include(i=>i.District).ThenInclude(i=>i.City).FirstOrDefault();
            shoppingCartVM.Order.CustomerAddressId = shoppingCartVM.Order.Address.Id;

            shoppingCartVM.Order.OdemeDurum = SD.Odeme_Yapilmadi;
            shoppingCartVM.Order.SiparisDurum = SD.Siparis_Olusturulmadi;


			foreach (var cart in shoppingCartVM.CartList)
			{
				shoppingCartVM.Order.OrderTotal = shoppingCartVM.Order.OrderTotal + (double)cart.SystemUser.Price;
			}
			return View(shoppingCartVM);
			
        }

		public IActionResult Remove(int cartId)
        {

            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();



            return RedirectToAction(nameof(OrderCard));

        }
    }
}
