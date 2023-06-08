using Hammal.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.Models
{
	public class ShoppingCart
	{
        public int Id { get; set; }
        public string? ApplicationUserId { get; set; }
		[ForeignKey(nameof(ApplicationUserId)), ValidateNever]
		public ApplicationUser? ApplicationUser { get; set; }
        public int SystemUserId { get; set; }
		[ForeignKey(nameof(SystemUserId)), ValidateNever]
		public SystemUser? SystemUser { get; set; }
		[NotMapped]
        public double OrderTotal { get; set; }
    }
}
