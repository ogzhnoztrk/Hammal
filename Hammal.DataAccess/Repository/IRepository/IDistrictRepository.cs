using Hammal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.DataAccess.Repository.IRepository
{
    public interface IDistrictRepository : IRepository<District>
    {
        void Update(District district);
    }
}
