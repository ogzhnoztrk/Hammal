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
    public class Address
    {
        [Key]
        public int Id { get; set; }
        
        public string? ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        //[Required]
        //public int CityId { get; set; }
        //[ForeignKey("CityId")]
        //[ValidateNever]
        //public City City { get; set; }

        //Şehirleri zaten ilçelerle bağlı olduğu için ve migration atarken hata verdiği için bağlantısnı çikardım 



        [Required]
        public int DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        [ValidateNever]
        public District District { get; set; }
        public string Street { get; set; }

    }
}
