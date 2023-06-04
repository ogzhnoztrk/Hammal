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
  public class SystemUser
  {
    [Key]
    public int? Id { get; set; }
    public int? CategoryId { get; set; }
    public int? AltCategoryId { get; set; }
    public string? Abilities { get; set; }
    public double? Price { get; set; }

    public string? ApplicationUserId { get; set; } = null;
    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }
  }
}
