using Microsoft.AspNetCore.Mvc;
using Projekat.BLL.Services.Interfaces;
using Projekat.DAL.Model;
using Projekat.Shared.Common;
using Projekat.Shared.Constants;
using Projekat.Shared.DTOs;

namespace Projekat.WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService= categoryService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ResponsePackage<IEnumerable<CategoryDTO>> response = _categoryService.GetAll();
            return View(response.Data);
        }


        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryDTO obj)
        {
            
            if (ModelState.IsValid)
            {
                ResponsePackage<bool> response= _categoryService.Create(obj.Category);
                
                return RedirectToAction("Index");
            }
            return View();
        }


        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            ResponsePackage<CategoryDTO> response = _categoryService.GetCategory(id);
            
            if (response.Status == ResponseStatus.OK)
            {

                return View(response.Data);

            }
                return NotFound();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryDTO obj)
        {
            if (ModelState.IsValid)
            {
                ResponsePackage<bool> response = _categoryService.Update(obj.CategoryId,obj.Category);
                if(response.Status==Shared.Constants.ResponseStatus.OK)
                    return RedirectToAction("Index");

                else
                    ModelState.AddModelError(String.Empty, response.Message);
            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            
            ResponsePackage<bool> response = _categoryService.Delete(id);

            if (response.Status == ResponseStatus.OK)
            {
                ResponsePackage<IEnumerable<CategoryDTO>> categories = _categoryService.GetAll();
                return View("Index",categories.Data);

            }else
            {
                ModelState.AddModelError(String.Empty, response.Message);
                return View("Index");
            }
        }
    }
}
