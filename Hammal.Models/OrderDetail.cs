using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.Models
{
	public class OrderDetail
	{
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId)), ValidateNever]
        public Order Order { get; set; }
        public int SystemUserId { get; set; }
		[ForeignKey(nameof(SystemUserId)), ValidateNever]
		public SystemUser SystemUser { get; set; }
    }
}
