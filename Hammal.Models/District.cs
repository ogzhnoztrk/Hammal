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
    public class District
    {
        [Key]
        public int Id { get; set; }
        public string Name{ get; set; }
        public int CityId { get; set; }
        [ForeignKey("CityId")]
        [ValidateNever]
        public virtual City City { get; set; }
    }
}
