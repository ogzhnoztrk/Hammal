using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.Models
{
    public class Ability
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Name { get; set; }
    }
}
