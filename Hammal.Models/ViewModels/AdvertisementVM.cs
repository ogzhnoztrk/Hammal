using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.Models.ViewModels
{
    public class AdvertisementVM
    {
        public Advertisement? Advertisement { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
    }
}
