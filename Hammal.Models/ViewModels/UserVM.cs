using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.Models.ViewModels
{
    public class UserVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<Address> Addresses{ get; set; }
        public IEnumerable<UserAbility> UserAbilities{ get; set; }
    }
}
