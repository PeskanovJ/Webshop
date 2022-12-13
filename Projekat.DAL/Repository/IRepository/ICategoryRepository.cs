using Projekat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.DAL.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
        void Save();
    }
}
