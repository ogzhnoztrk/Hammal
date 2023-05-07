using Hammal.DataAccess.Data;
using Hammal.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            Advertisement = new AdvertisementRepository(_db);
            AltCategory = new AltCategoryRepository(_db);
        }

        public ICategoryRepository Category { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IAdvertisementRepository Advertisement { get; private set; }
        public IAltCategoryRepository AltCategory { get; private set; }
    
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
