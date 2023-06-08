using Hammal.DataAccess.Data;
using Hammal.DataAccess.Repository.IRepository;
using Hammal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.DataAccess.Repository
{
  public class UnitOfWork : IUnitOfWork
  {
    private static ApplicationDbContext _db;

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
      UserAbility = new UserAbilityRepository(_db);
      SystemUser = new SystemUserRepository(_db);
      Order = new OrderRepository(_db);
      OrderDetail = new OrderDetailRepository(_db);
      ShoppingCart = new ShoppingCartRepository(_db);

    }
    public static IRepository<T> GetRepository<T>() where T : class
    {
      return new Repository<T>(_db);
    }

    public ICategoryRepository Category { get; private set; }
    public IApplicationUserRepository ApplicationUser { get; private set; }
    public IAdvertisementRepository Advertisement { get; private set; }
    public IAltCategoryRepository AltCategory { get; private set; }
    public ICityRepository City { get; set; }
    public IDistrictRepository District { get; set; }
    public IAddressRepository Address { get; set; }
    public IUserAbilityRepository UserAbility { get; set; }
    public ISystemUserRepository SystemUser { get; set; }
    public IOrderRepository Order{ get; set; }
    public IOrderDetailRepository OrderDetail{ get; set; }
    public IShoppingCartRepository ShoppingCart{ get; set; }


    public void Save()
    {
      _db.SaveChanges();
    }
    public async Task<int> SaveAsync()
    {

      var result = await _db.SaveChangesAsync();

      return result;
    }

  
  }
}
