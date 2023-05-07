using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IAdvertisementRepository Advertisement { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IAltCategoryRepository AltCategory { get; }
        void Save();
    }
}
