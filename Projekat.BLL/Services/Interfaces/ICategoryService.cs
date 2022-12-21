using Projekat.DAL.Model;
using Projekat.Shared.Common;
using Projekat.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.BLL.Services.Interfaces
{
    public interface ICategoryService
    {
        ResponsePackage<bool> Create(string categoryName);
        ResponsePackage<bool> Update(int categoryId,string newName);
        ResponsePackage<bool> Delete(int categoryId);

        ResponsePackage<IEnumerable<CategoryDTO>> GetAll();
        ResponsePackage<CategoryDTO> GetCategory(int? categoryId);
    }
}
