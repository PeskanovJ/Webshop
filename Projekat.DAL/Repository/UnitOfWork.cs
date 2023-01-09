using Projekat.DAL;
using Projekat.DAL.Model;
using Projekat.DAL.Repository;
using Projekat.DAL.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
          
            Category = new CategoryRepository(_db);
            Item = new ItemRepository(_db);
            
            User= new UserRepository(_db);
           
        }
        public ICategoryRepository Category { get; private set; }
        public IItemRepository Item { get; set; }
        public IUserRepository User { get; set; }
        public void Unfollow(Following following)
        {
            _db.Followings.Remove(following);
            _db.SaveChanges();
        }

        public void Save()
        {
           
           _db.SaveChanges();
        }
    }
}
