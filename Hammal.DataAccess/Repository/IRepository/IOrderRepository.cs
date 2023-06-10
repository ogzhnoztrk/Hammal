using Hammal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.DataAccess.Repository.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order order);
		void UpdateStatus(int id, string orderStatus, string paymentStatus = null);
		void UpdateStripePaymentID(int id, string sessionId, string paymentItentId);

    }
}
