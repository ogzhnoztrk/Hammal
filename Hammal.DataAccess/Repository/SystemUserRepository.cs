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
    public class SystemUserRepository : Repository<SystemUser>, ISystemUserRepository
    {
        private readonly ApplicationDbContext _db;

        public SystemUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(SystemUser obj)
        {
            _db.SystemUsers.Update(obj);
        }
    }
}
