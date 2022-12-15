using Projekat.BLL.Services.Interfaces;
using Projekat.DAL.Model;
using Projekat.DAL.Repository;
using Projekat.DAL.Repository.IRepository;
using Projekat.Shared.Common;
using Projekat.Shared.Constants;
using Projekat.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.BLL.Services.Implementations
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ResponsePackage<bool> AddItem(ItemDTO itemDTO)
        {
            throw new NotImplementedException();
        }

        public ResponsePackage<bool> DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        public ResponsePackage<ItemDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public ResponsePackage<ItemDTO> GetItem(int id)
        {
            Item i= _unitOfWork.Item.GetFirstOrDefault(x => x.Id == id);

            if (i == null)
                return new ResponsePackage<ItemDTO>(null, ResponseStatus.NotFound, "Item not found");
            else
                return new ResponsePackage<ItemDTO>(new ItemDTO { 
                    Title=i.Title, 
                    Category= i.Category, 
                    City=i.City, 
                    Description=i.Description, 
                    Make=i.Make,
                    Model=i.Model,
                    Price=i.Price,
                }, ResponseStatus.OK, "Item found");
        }

        public ResponsePackage<bool> UpdateItem(ItemDTO itemDTO)
        {
            throw new NotImplementedException();
        }
    }
}
