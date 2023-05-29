using Hammal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.DataAccess.Repository.IRepository
{
    public interface IAddressRepository : IRepository<Address>
    {
        void Update(Address address);
    } 
}
