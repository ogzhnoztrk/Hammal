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
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OrderCard(Order order)
        {

            OrderVM orderVM = new OrderVM();
            orderVM.Order = order;
            var existingRecord = _unitOfWork.Order.Find(x=>x.Id == order.Id).FirstOrDefault();
            var SystemUser = _unitOfWork.SystemUser.Find(i=>i.Id == order.SystemUserId).Include(i=>i.AltCategory).ThenInclude(i=>i.Category).FirstOrDefault();
            if (existingRecord == null)
            {
                orderVM.SystemUser = SystemUser;
                _unitOfWork.Order.Add(order);

            }
            
            
            _unitOfWork.Save();

            return View(orderVM);

        }

        public IActionResult Remove(int cartId)
        {

            var cart = _unitOfWork.Order.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.Order.Remove(cart);
            _unitOfWork.Save();



            return RedirectToAction(nameof(OrderCard));

        }
    }
}
