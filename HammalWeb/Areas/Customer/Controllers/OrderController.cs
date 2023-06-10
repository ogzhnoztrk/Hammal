using Hammal.DataAccess.Repository;
using Hammal.DataAccess.Repository.IRepository;
using Hammal.Models;
using Hammal.Models.ViewModels;
using Hammal.Utilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace HammalWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
		[BindProperty]
		public ShoppingCartVM shoppingCartVM { get; set; }
		private readonly IUnitOfWork _unitOfWork;
		private readonly IRepository<Order> _orderRepo;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
						_orderRepo = UnitOfWork.GetRepository<Order>();

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


		[HttpPost]
		[ActionName("Summary")]
		[ValidateAntiForgeryToken]
		public IActionResult SummaryPOST()
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			shoppingCartVM.CartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "SystemUser");

			shoppingCartVM.Order.OrderDate = System.DateTime.Now;
			shoppingCartVM.Order.CustomerId = claim.Value;

			foreach (var cart in shoppingCartVM.CartList)
			{

				shoppingCartVM.Order.OrderTotal = shoppingCartVM.Order.OrderTotal + (double)cart.SystemUser.Price;

			}
			ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);

			//if (applicationUser.CompanyId.GetValueOrDefault() == 0)
			//{

			//	ShoppingCartVM.Order.PaymentStatus = SD.PaymentStatusPending;
			//	ShoppingCartVM.Order.OrderStatus = SD.StatusPending;
			//}
			if(claim != null)
			{
				shoppingCartVM.Order.OdemeDurum = SD.Odeme_PaymentStatusDelayedPayment;
				shoppingCartVM.Order.SiparisDurum= SD.Siparis_StatusApproved;
			}

			_unitOfWork.Order.Add(shoppingCartVM.Order);
			_unitOfWork.Save();

			foreach (var cart in shoppingCartVM.CartList)
			{
				OrderDetail orderDetail = new()
				{
					SystemUserId = cart.SystemUserId,
					OrderId = shoppingCartVM.Order.Id,
					
				};
				_unitOfWork.OrderDetail.Add(orderDetail);
				_unitOfWork.Save();
			}


			//StripeOdeme
			if (claim != null)
			{

				//stripe Settings
				var domain = "https://localhost:44394";
				var options = new SessionCreateOptions
				{
					PaymentMethodTypes = new List<string>
				{
				  "card"
				},
					LineItems = new List<SessionLineItemOptions>(),
					Mode = "payment",
					SuccessUrl = domain + $"/customer/Order/OrderConfirmation?id={shoppingCartVM.Order.Id}",
					CancelUrl = domain + $"/customer/Order/OrderCart",
				};

				foreach (var item in shoppingCartVM.CartList)
				{

					var sessionLineItem = new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
                            UnitAmount = (long)(item.SystemUser.Price * 100),
                            Currency = "try",
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = item.SystemUser.Abilities,
							},
						},
						Quantity =1,
					};
					options.LineItems.Add(sessionLineItem);
				}

				var service = new SessionService();          
                Session session = service.Create(options);

                _unitOfWork.Order.UpdateStripePaymentID(shoppingCartVM.Order.Id, session.Id, session.PaymentIntentId);
				_unitOfWork.Save();
				Response.Headers.Add("Location", session.Url);
				return new StatusCodeResult(303);
			}
			else
			{
				return RedirectToAction("OrderConfirmation", "Cart", new { id = shoppingCartVM.Order.Id });

			}
		}

		//OrderConfirmation
		//OrderConfirmation
		public IActionResult OrderConfirmation(int id)
		{
			Order order= _unitOfWork.Order.GetFirstOrDefault(x => x.Id == id, includeProperties: "ApplicationUser");

			if (order.OdemeDurum != SD.Odeme_PaymentStatusDelayedPayment)
			{
				var service = new SessionService();
				Session session = service.Get(order.SessionId);
				//check the stripe status
				if (session.PaymentStatus.ToLower() == "paid")
				{
					_unitOfWork.Order.UpdateStripePaymentID(id, order.SessionId, session.PaymentIntentId);
					_unitOfWork.Order.UpdateStatus(id, SD.Siparis_StatusApproved, SD.Odeme_PaymentStatusApproved); ;
					_unitOfWork.Save();
				}
        }

			//_emailSender.SendEmailAsync(order.ApplicationUser.Email, "New Order- Bulky Book", "<p>New Order Created</p>");
			List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == order.CustomerId).ToList();
			_unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
			_unitOfWork.Save();


			return View(id);

		}

    public async Task<IActionResult> OrderDenied(int id)
    {
      Order order = await _orderRepo.GetAll().Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.Id == id);


      if (order.OdemeDurum == SD.Odeme_PaymentStatusDelayedPayment)
      {
        var service = new SessionService();
        Session session = await service.GetAsync(order.SessionId);
        //check the stripe status
        if (session.PaymentStatus.ToLower() == "paid")
        {
          _unitOfWork.Order.UpdateStripePaymentID(id, order.SessionId, session.PaymentIntentId);
          _unitOfWork.Order.UpdateStatus(id, SD.Siparis_StatusCancelled, SD.Odeme_PaymentStatusRejected); ;
          _unitOfWork.Save();
        }
      }

      //_emailSender.SendEmailAsync(order.ApplicationUser.Email, "New Order- Bulky Book", "<p>New Order Created</p>");
      //List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == order.CustomerId).ToList();
      //_unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
      //_unitOfWork.Save();


      return View(id);

    }



    public async Task<IActionResult> GetOrderList(int? applicationUserId)
    {
     
			var aa = await _orderRepo.GetAll().ToListAsync();

    
      return View("OrderList",aa);

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
