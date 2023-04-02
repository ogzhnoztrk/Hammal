using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.Models
{
    public class Advertisement
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [Required]
        public string AdvertiserID { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.Now;
        [DisplayName("Last Date")]

        public DateTime LastDate { get; set; }

    }
}
