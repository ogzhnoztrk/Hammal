using Hammal.DataAccess.Data;
using Hammal.DataAccess.Repository.IRepository;
using Hammal.Models;
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
            City = new CityRepository(_db);
            District = new DistrictRepository(_db);
            Address = new AddressRepository(_db);

        }

        public ICategoryRepository Category { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IAdvertisementRepository Advertisement { get; private set; }
        public IAltCategoryRepository AltCategory { get; private set; }
        public ICityRepository City { get; set; }
        public IDistrictRepository District { get; set; }
        public IAddressRepository Address { get; set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
