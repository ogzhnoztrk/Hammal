using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.Models.Dtos
{
  public class SystemUserDto
  {
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? CityName { get; set; }
    public string? DistrictName { get; set; }
    public string? CategoryName { get; set; }
    public string? AltCategoryName { get; set; }
    public string? Abilities { get; set; }
    public double? Price { get; set; }



  }
}
