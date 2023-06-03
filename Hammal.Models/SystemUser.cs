using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.Models
{
    public class SystemUser
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public int? AddressId { get; set; }
        public string? PhoneNumber { get; set; }
        public int? AltCategoryId { get; set; }
        public int? CategoryId { get; set; }
        
      
  
  
  }
}
