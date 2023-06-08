using Hammal.DataAccess.Repository;
using Hammal.DataAccess.Data;
using Hammal.DataAccess.Repository.IRepository;
using Hammal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.DataAccess.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Order obj)
        {
            _db.Orders.Update(obj);
        }
		public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
		{
			var orderFromDb = _db.Orders.FirstOrDefault(x => x.Id == id);
			if (orderFromDb != null)
			{
				orderFromDb.SiparisDurum = orderStatus;
				if (paymentStatus != null)
				{
					orderFromDb.OdemeDurum = paymentStatus;
				}
			}
		}

		public void UpdateStripePaymentID(int id, string sessionId, string paymentItentId)
		{
			var orderFromDb = _db.Orders.FirstOrDefault(x => x.Id == id);
			orderFromDb.PaymentDate = DateTime.Now;
			orderFromDb.SessionId = sessionId;
			orderFromDb.PaymentIntentId = paymentItentId;



		}
	}
}
