using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.DAL.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IItemRepository Product { get; }
        IUserRepository User { get; }
        ICategoryRepository Category { get; }
        void Save();
    }
}
