using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Projekat.BLL.Services.Interfaces;
using Projekat.DAL.Model;
using Projekat.DAL.Repository;
using Projekat.DAL.Repository.IRepository;
using Projekat.Shared.Common;
using Projekat.Shared.Constants;
using Projekat.Shared.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            item.UserId = newItemDTO.UserId;
            item.Created = DateTime.Now;
            item.Price = newItemDTO.Price;
            item.City = newItemDTO.City;
            item.Make= newItemDTO.Make;
            item.Model= newItemDTO.Model;
            item.CategoryId= newItemDTO.CategoryId;
            item.FollowedItems = new List<Following>();
            if(newItemDTO.Images!= null)
            {
                item.Images = new List<Image>();
                foreach(var image in newItemDTO.Images)
                {
                    item.Images.Add(new Image() { IsMainImage = image.IsMainImage, Order = image.Order, Url ="\\"+image.Url.Substring(image.Url.IndexOf("img")) });
                }
            }
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

            Item item = _unitOfWork.Item.GetFirstOrDefault(u => u.Id == id);
           
            item.IsDeleted = true;

            _unitOfWork.Item.Update(item);
            _unitOfWork.Save();

            return new ResponsePackage<bool>(true, ResponseStatus.OK, "Item deleted");
        }

        public ResponsePackage<IEnumerable<ItemDTO>> GetAll(string? includeProperties = null)
        {
            var list = _unitOfWork.Item.GetAll(includeProperties:includeProperties);
            var retList= new List<ItemDTO>();
            foreach(var item in list)
            {
                if (!item.IsDeleted)
                {
                    ItemDTO ItemDTO = new ItemDTO()
                    {
                        Title = item.Title,
                        Description = item.Description,
                        Price = item.Price,
                        City = item.City,
                        Id = item.Id,
                        CategoryId = item.CategoryId,
                        Make = item.Make,
                        Model = item.Model,
                        UserId = item.UserId,
                        Created= item.Created,
                    };
                    if (item.Images != null)
                    {
                        ItemDTO.Images = new List<ImageDTO>();
                        foreach (var image in item.Images) { ItemDTO.Images.Add(new ImageDTO() { IsMainImage = image.IsMainImage, Order = image.Order, Url = image.Url }); }
                    }
                    if (item.FollowedItems != null)
                    {
                        ItemDTO.Following = new List<FollowingDTO>();
                        foreach (var following in item.FollowedItems) { if (!following.IsDeleted) ItemDTO.Following.Add(new FollowingDTO() { UserId = following.UserId, ItemId = following.ItemId }); }
                    }

                    retList.Add(ItemDTO);
                }
            }
            return new ResponsePackage<IEnumerable<ItemDTO>>(retList,ResponseStatus.OK,"All items");
        }

        public ResponsePackage<IEnumerable<ItemDTO>> GetLatest(string? includeProperties = null)
        {
            var list = _unitOfWork.Item.GetAll(includeProperties: includeProperties);
            list = list.OrderByDescending(x => x.Created).ToList();
            list= list.Where(x=>!x.IsDeleted).ToList();
            var retList = new List<ItemDTO>();
            for(int i=0;i<(list.Count()>10?10:list.Count());i++)
            {
                Item item = list.ElementAt(i);
                if (!item.IsDeleted)
                {
                    ItemDTO ItemDTO = new ItemDTO()
                    {
                        Title = item.Title,
                        Description = item.Description,
                        Price = item.Price,
                        City = item.City,
                        Id = item.Id,
                        CategoryId = item.CategoryId,
                        Make = item.Make,
                        Model = item.Model,
                        UserId = item.UserId,
                        Created= item.Created,
                    };
                    if (item.Images != null)
                    {
                        ItemDTO.Images = new List<ImageDTO>();
                        foreach (var image in item.Images) { ItemDTO.Images.Add(new ImageDTO() { IsMainImage = image.IsMainImage, Order = image.Order, Url = image.Url }); }
                    }
                    if (item.FollowedItems != null)
                    {
                        ItemDTO.Following = new List<FollowingDTO>();
                        foreach (var following in item.FollowedItems) { if (!following.IsDeleted) ItemDTO.Following.Add(new FollowingDTO() { UserId = following.UserId, ItemId = following.ItemId }); }
                    }
                    retList.Add(ItemDTO);
                }
            }
            return new ResponsePackage<IEnumerable<ItemDTO>>(retList, ResponseStatus.OK, "10 latest items");
        }

        public ResponsePackage<ItemDTO> GetItem(int id, string? includeProperties = null)
        {
            Item i= _unitOfWork.Item.GetFirstOrDefault(x => x.Id == id, includeProperties: "Images,FollowedItems");

            if (i == null || i.IsDeleted)
                return new ResponsePackage<ItemDTO>(null, ResponseStatus.NotFound, "Item not found");
            else
            {
                ItemDTO retItem = new ItemDTO
                {
                    Id= id,
                    Title = i.Title,
                    CategoryId = i.CategoryId,
                    City = i.City,
                    Description = i.Description,
                    Make = i.Make,
                    Model = i.Model,
                    Price = i.Price,
                    Created= i.Created,
                    UserId= i.UserId,
                };
                if (i.Images != null)
                {
                    retItem.Images = new List<ImageDTO>();
                    foreach (var image in i.Images) { retItem.Images.Add(new ImageDTO() { IsMainImage = image.IsMainImage, Order = image.Order, Url = image.Url }); }
                }

                if (i.FollowedItems != null)
                {
                    retItem.Following = new List<FollowingDTO>();
                    foreach (var following in i.FollowedItems) { if(!following.IsDeleted)retItem.Following.Add(new FollowingDTO() { UserId = following.UserId, ItemId=following.ItemId }); }
                }
                return new ResponsePackage<ItemDTO>(retItem, ResponseStatus.OK, "Item found");
            }
        }


        public ResponsePackage<bool> UpdateItem(ItemDTO ItemDTO)
        {
            Item item = _unitOfWork.Item.GetFirstOrDefault(u => u.Id == ItemDTO.Id);
            item.Title = ItemDTO.Title;
            item.Description = ItemDTO.Description;
            item.City = ItemDTO.City;
            item.Make = ItemDTO.Make;
            item.Model = ItemDTO.Model;
            item.Price = ItemDTO.Price;

            _unitOfWork.Item.Update(item);
            _unitOfWork.Save();

            return new ResponsePackage<bool>(true, ResponseStatus.OK, "Item changed");
        }

        public ResponsePackage<IEnumerable<ItemDTO>> GetByUser(int UserId, string? includeProperties = null)
        {
            var list = _unitOfWork.Item.GetAll(u=>u.UserId==UserId,includeProperties: includeProperties);
            var retList = new List<ItemDTO>();
            foreach (var item in list)
            {
                if (!item.IsDeleted)
                {
                    ItemDTO ItemDTO = new ItemDTO()
                    {
                        Title = item.Title,
                        Description = item.Description,
                        Price = item.Price,
                        City = item.City,
                        Id = item.Id,
                        CategoryId = item.CategoryId,
                        Make = item.Make,
                        Model = item.Model,
                        UserId = item.UserId,
                        Created=item.Created,
                    };
                    if (item.Images != null)
                    {
                        ItemDTO.Images = new List<ImageDTO>();
                        foreach (var image in item.Images) { ItemDTO.Images.Add(new ImageDTO() { IsMainImage = image.IsMainImage, Order = image.Order, Url = image.Url }); }
                    }
                    retList.Add(ItemDTO);
                }
            }
            return new ResponsePackage<IEnumerable<ItemDTO>>(retList, ResponseStatus.OK, "All items from User");
        }

        public ResponsePackage<IEnumerable<ItemDTO>> GetFollowed(int UserId, string? includeProperties = null)
        {
            var list = _unitOfWork.Item.GetAll(includeProperties: "Images,FollowedItems");
            var retList = new List<ItemDTO>();
            foreach (var item in list)
            {
                if (item.FollowedItems.FirstOrDefault(u=>u.UserId==UserId) != null)
                {
                    ItemDTO ItemDTO = new ItemDTO()
                    {
                        Title = item.Title,
                        Description = item.Description,
                        Price = item.Price,
                        City = item.City,
                        Id = item.Id,
                        CategoryId = item.CategoryId,
                        Make = item.Make,
                        Model = item.Model,
                        UserId = item.UserId,
                        Created = item.Created,
                    };
                    if (item.Images != null)
                    {
                        ItemDTO.Images = new List<ImageDTO>();
                        foreach (var image in item.Images) { ItemDTO.Images.Add(new ImageDTO() { IsMainImage = image.IsMainImage, Order = image.Order, Url = image.Url }); }
                    }
                    retList.Add(ItemDTO);
                }
            }
            return new ResponsePackage<IEnumerable<ItemDTO>>(retList, ResponseStatus.OK, "All items you follow");
        }
        private IEnumerable<Item> nameFilter(string name,IEnumerable<Item> list)
        {
            List<Item> ret = new List<Item>();

            foreach (var item in list)
            {
                if (item.Title.ToLower().Contains(name.ToLower()))
                    ret.Add(item);
            }
            return ret;
        }
        private IEnumerable<Item> modelFilter(string model, IEnumerable<Item> list)
        {
            List<Item> ret = new List<Item>();

            foreach (var item in list)
            {
                if (item.Model.ToLower().Contains(model.ToLower()))
                    ret.Add(item);
            }
            return ret;
        }
        private IEnumerable<Item> makeFilter(string make, IEnumerable<Item> list)
        {
            List<Item> ret = new List<Item>();

            foreach (var item in list)
            {
                if (item.Make.ToLower().Contains(make.ToLower()))
                    ret.Add(item);
            }
            return ret;
        }
        public ResponsePackage<IEnumerable<ItemDTO>> Filter(string name, string model, string make, string? includeProperties = null)
        {
            var list = _unitOfWork.Item.GetAll(includeProperties: includeProperties);
            if (name != null)
                list = nameFilter(name,list);
            if (model != null)
                list = modelFilter(model, list);
            if (make != null)
                list = makeFilter(make, list);

            var retList = new List<ItemDTO>();
            foreach (var item in list)
            {
                if (!item.IsDeleted)
                {
                    ItemDTO ItemDTO = new ItemDTO()
                    {
                        Title = item.Title,
                        Description = item.Description,
                        Price = item.Price,
                        City = item.City,
                        Id = item.Id,
                        CategoryId = item.CategoryId,
                        Make = item.Make,
                        Model = item.Model,
                        UserId = item.UserId,
                        Created = item.Created,
                    };
                    if (item.Images != null)
                    {
                        ItemDTO.Images = new List<ImageDTO>();
                        foreach (var image in item.Images) { ItemDTO.Images.Add(new ImageDTO() { IsMainImage = image.IsMainImage, Order = image.Order, Url = image.Url }); }
                    }
                    retList.Add(ItemDTO);
                }
            }
            return new ResponsePackage<IEnumerable<ItemDTO>>(retList, ResponseStatus.OK, "All items");
        }
    }
}
