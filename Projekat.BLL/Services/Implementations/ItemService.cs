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

        public ResponsePackage<bool> AddItem(ItemDTO newItemDTO)
        {
            Item item = new Item();
            item.Title = newItemDTO.Title;
            item.Description = newItemDTO.Description;
            
            item.Created = DateTime.Now;
            item.Price = newItemDTO.Price;
            item.City = newItemDTO.City;
            
            try
            {
                _unitOfWork.Item.Add(item);
                _unitOfWork.Save();

                return new ResponsePackage<bool>(true, ResponseStatus.OK, "Item added successfully");
            }catch(Exception ex)
            {
                return new ResponsePackage<bool>(false, ResponseStatus.InternalServerError, "There was an error while adding an item:" + ex.Message);
            }
        }

        public ResponsePackage<bool> DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        public ResponsePackage<IEnumerable<ItemDTO>> GetAll()
        {
            var list = _unitOfWork.Item.GetAll();
            var retList= new List<ItemDTO>();
            foreach(var item in list)
            {
                ItemDTO ItemDTO= new ItemDTO()
                {
                    Title = item.Title,
                    Description = item.Description,
                    Price= item.Price,
                    City= item.City,
                    Id= item.Id,
                    CategoryId= item.CategoryId,
                    Make= item.Make,
                    Model= item.Model,
                };
                retList.Add(ItemDTO);
            }
            return new ResponsePackage<IEnumerable<ItemDTO>>(retList,ResponseStatus.OK,"All items");
        }

        public ResponsePackage<ItemDTO> GetItem(int id)
        {
            Item i= _unitOfWork.Item.GetFirstOrDefault(x => x.Id == id);

            if (i == null)
                return new ResponsePackage<ItemDTO>(null, ResponseStatus.NotFound, "Item not found");
            else
                return new ResponsePackage<ItemDTO>(new ItemDTO { 
                    Title=i.Title, 
                    CategoryId= i.CategoryId, 
                    City=i.City, 
                    Description=i.Description, 
                    Make=i.Make,
                    Model=i.Model,
                    Price=i.Price,
                }, ResponseStatus.OK, "Item found");
        }

        public ResponsePackage<bool> UpdateItem(NewItemDTO newItemDTO)
        {
            throw new NotImplementedException();
        }
    }
}
