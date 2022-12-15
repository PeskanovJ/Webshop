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
        //Task<ResponsePackage<bool>> RegisterUser(UserDTO userDTO);
        ResponsePackage<ItemDTO> GetItem(int id);
        ResponsePackage<bool> AddItem(ItemDTO itemDTO);
        ResponsePackage<bool> UpdateItem(ItemDTO itemDTO);
        ResponsePackage<ItemDTO> GetAll();
        ResponsePackage<bool> DeleteItem(int id);
    }
}
