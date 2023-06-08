using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.Models.ViewModels
{
    public class OrderVM
    {
        public IEnumerable<Order>? Order { get; set; }
        public SystemUser? SystemUser { get; set; }
    }
}
