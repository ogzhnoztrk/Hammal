using Hammal.DataAccess.Repository;
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
    public class UserAbilityRepository : Repository<UserAbility>, IUserAbilityRepository
    {
        private readonly ApplicationDbContext _db;

        public UserAbilityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(UserAbility obj)
        {
            _db.UserAbilities.Update(obj);
        }
    }
}
