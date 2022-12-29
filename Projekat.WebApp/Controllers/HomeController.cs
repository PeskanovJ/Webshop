using Microsoft.AspNetCore.Mvc;
using Projekat.BLL.Services.Interfaces;
using Projekat.Shared.Common;
using Projekat.Shared.DTOs;

namespace Projekat.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IItemService _itemService;
        public HomeController(IItemService itemService)
        {
            _itemService= itemService; 
        }
        public IActionResult Index()
        {
            ResponsePackage<IEnumerable<ItemDTO>> response = _itemService.GetLatest("Images");
            return View(response.Data);
        }
    }
}
