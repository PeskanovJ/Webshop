using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Projekat.BLL.Services.Interfaces;
using Projekat.DAL.Repository.IRepository;
using Projekat.Shared.Common;
using Projekat.Shared.Constants;
using Projekat.Shared.DTOs;

namespace Projekat.WebApp.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IWebHostEnvironment _hostEnviroment;
        public ItemController(IItemService itemService, IWebHostEnvironment hostEnviroment)
        {
            _itemService = itemService;
            _hostEnviroment = hostEnviroment;
            
        }

        public IActionResult Index()
        {
            return View();
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
                return View(new ItemDTO());



        }
        //POST
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Upsert(ProductVM obj, IFormFile? file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string wwwRootPath = _hostEnviroment.WebRootPath;
        //        if (file != null)
        //        {
        //            string fileName = Guid.NewGuid().ToString();
        //            var uploads = Path.Combine(wwwRootPath, @"images\products");
        //            var extension = Path.GetExtension(file.FileName);

        //            if (obj.Product.ImageUrl != null)
        //            {
        //                var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
        //                if (System.IO.File.Exists(oldImagePath))
        //                {
        //                    System.IO.File.Delete(oldImagePath);
        //                }
        //            }


        //            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
        //            {
        //                file.CopyTo(fileStreams);
        //            }
        //            obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
        //        }
        //        if (obj.Product.Id == 0)
        //        {
        //            _unitOfWork.Product.Add(obj.Product);
        //            TempData["success"] = "Product created successfully";
        //        }
        //        else
        //        {
        //            _unitOfWork.Product.Update(obj.Product);
        //            TempData["success"] = "Product edited successfully";
        //        }


        //        _unitOfWork.Save();

        //        return RedirectToAction("Index");
        //    }
        //    TempData["error"] = "There was an error";
        //    return RedirectToAction("Index");
        //}

        
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
