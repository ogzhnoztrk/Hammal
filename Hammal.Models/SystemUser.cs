using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.Models
{
  public class SystemUser
  {
    [Key]
    public string? Id { get; set; }

    [ForeignKey("ApplicationUser")]
    public string? ApplicationUserId { get; set; }

    public int? AltCategoryId { get; set; }
    public int? CategoryId { get; set; }

    public ApplicationUser ApplicationUser { get; set; }
  }

}
