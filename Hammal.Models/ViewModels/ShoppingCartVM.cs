using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.Models.ViewModels
{
	public class ShoppingCartVM
	{
        public IEnumerable<ShoppingCart> CartList{ get; set; }
        public Order Order { get; set; }

    }
}
