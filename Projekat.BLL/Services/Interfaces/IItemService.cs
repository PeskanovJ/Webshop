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
    public interface IItemService
    {
        ResponsePackage<ItemDTO> GetItem(int id, string? includeProperties = null);
        ResponsePackage<bool> AddItem(ItemDTO itemDTO);
        ResponsePackage<bool> UpdateItem(ItemDTO itemDTO);
        ResponsePackage<IEnumerable<ItemDTO>> GetAll(string? includeProperties = null);
        ResponsePackage<IEnumerable<ItemDTO>> Filter(string name,string model,string make,string? includeProperties = null);
        ResponsePackage<IEnumerable<ItemDTO>> GetByUser(int UserId,string? includeProperties = null);
        ResponsePackage<IEnumerable<ItemDTO>> GetLatest(string? includeProperties = null);
        ResponsePackage<bool> DeleteItem(int id);
    }
}
