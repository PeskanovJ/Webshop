using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Projekat.BLL.Services.Interfaces;
using Projekat.DAL.Repository.IRepository;
using Projekat.Shared.Common;
using Projekat.Shared.Constants;
using Projekat.Shared.DTOs;
using System.Web;

namespace Projekat.WebApp.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _hostEnviroment;
        public ItemController(IItemService itemService,ICategoryService categoryService, IWebHostEnvironment hostEnviroment)
        {
            _categoryService= categoryService;
            _itemService = itemService;
            _hostEnviroment = hostEnviroment;
            
        }

        public IActionResult Index()
        {
            ResponsePackage<IEnumerable<ItemDTO>> response = _itemService.GetAll();
            return View(response.Data);
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            if (id != null)
            {
                int itemId = id.Value;
                ResponsePackage<ItemDTO> response = _itemService.GetItem(itemId);
                if (response.Status == ResponseStatus.OK)
                    return View(response.Data);
                else
                {
                    ModelState.AddModelError(String.Empty, "There was an error while searching for item. Error:" + response.Message);
                    return View();
                }
            }
            else
                return View(new NewItemDTO()
                {
                    ItemDTO = new ItemDTO(),
                    CategoryList = _categoryService.GetAll().Data.Select(i => new SelectListItem
                    {
                        Text = i.Category,
                        Value = i.CategoryId.ToString()
                    })
                });
            



        }
        //POST
        [HttpPost]
        public IActionResult Upsert(IFormCollection dto)
        {
            ItemDTO itemDTO = JsonConvert.DeserializeObject<ItemDTO>(dto["ItemDTO"]);
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnviroment.WebRootPath;
                if (dto.Files != null)
                {
                    string fileFolderName = Guid.NewGuid().ToString().Replace('-', '_');
                    var uploads = Path.Combine(wwwRootPath, @"img\items\" + fileFolderName);
                    Directory.CreateDirectory(uploads);
                    int i = 0;
                    itemDTO.Images= new List<ImageDTO>();
                    foreach (var file in dto.Files)
                    {
                        var extension = Path.GetExtension(file.FileName);

                        using (var fileStreams = new FileStream(Path.Combine(uploads, file.FileName + extension), FileMode.Create))
                        {
                            file.CopyTo(fileStreams);
                            itemDTO.Images.Add(new ImageDTO { IsMainImage = false,  Order=i++, Url= Path.Combine(uploads, file.FileName) });
                        }
                    }
                    _itemService.AddItem(itemDTO);
                }
                //if (itemDTO.Id == 0)
                //   // _itemService.AddItem(itemDTO);
                //else
                //    //_itemService.UpdateItem(newItemDTO);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var producList = _itemService.GetAll();
            return Json(new { data = producList });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var obj = _itemService.GetItem(id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            //string wwwRootPath = _hostEnviroment.WebRootPath;
            //var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
            //if (System.IO.File.Exists(oldImagePath))
            //{
            //    System.IO.File.Delete(oldImagePath);
            //}

            ResponsePackage<bool> response = _itemService.DeleteItem(id);
            
            return Json(new { success = true, message = response.Message });



        }
        
    }
}
