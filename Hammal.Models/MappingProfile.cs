using AutoMapper;
using Hammal.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.Models
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      // Add as many of these lines as you need to map your objects
      CreateMap<Category, CategoryListDto>();
    }
  }
}
