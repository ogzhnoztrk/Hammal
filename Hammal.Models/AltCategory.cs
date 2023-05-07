using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.Models
{
    public class AltCategory
    {

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(70)]
        public string? Name { get; set; }
        public string? ImgPath { get; set; }
         public string? Aciklama { get; set; }
        // Foreign key property
        public int CategoryId { get; set; }

        // Navigation property
        [ForeignKey("CATEGORY_ID")]
        public Category Category { get; set; }

  }
}
