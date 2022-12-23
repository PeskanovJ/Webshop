using Projekat.DAL.Repository.IRepository;
using Projekat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekat.DAL.Repository;

namespace Projekat.DAL.Repository
{
    public class ItemRepository : Repository<Item> , IItemRepository
    {

        private ApplicationDbContext _db;

        public ItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Item obj)
        {
            var objFromDb = _db.Items.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title= obj.Title;
            }
        }
    }
}
