using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

       
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string? SiparisDurum { get; set; }/*Hazırlanıyor, tamamlandı Gibisinden*/
        public string? OdemeDurum { get; set; } /*ödeme sisteminden gelen durum bilgsi*/

        public double OrderTotal { get; set; }

        public int CustomerAddressId { get; set; }
		[ForeignKey("CustomerAddressId")]
		[ValidateNever]
		public Address Address { get; set; }
    }
}
