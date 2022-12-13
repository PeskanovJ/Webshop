using Projekat.DAL.Repository.IRepository;
using Projekat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.DAL.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db) :base(db)
        {
            _db=db;
        }

        public void Update(User obj)
        {
            _db.Users.Update(obj);
        }

    }
}
