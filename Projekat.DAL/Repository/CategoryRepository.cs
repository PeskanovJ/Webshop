﻿using Projekat.DAL.Repository.IRepository;
using Projekat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.DAL.Repository
{
    public class CategoryRepository : Repository<Category> , ICategoryRepository
    {

        private ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        
    }
}
