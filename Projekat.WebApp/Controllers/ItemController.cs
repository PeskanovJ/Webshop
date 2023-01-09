using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Projekat.BLL.Services.Interfaces;
using Projekat.DAL.Model;
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
        private readonly IUserService _userService;
        public ItemController(IItemService itemService, ICategoryService categoryService, IWebHostEnvironment hostEnviroment, IUserService userService)
        {
            _categoryService = categoryService;
            _itemService = itemService;
            _hostEnviroment = hostEnviroment;
            _userService = userService;
        }
        public IActionResult Index()
        {
            ResponsePackage<IEnumerable<ItemDTO>> response = _itemService.GetAll("Images");
            return View(response.Data);
        }

        [HttpGet]
        [Route("api/items")]
        public List<ItemDTO> Get()
        {
            ResponsePackage<IEnumerable<ItemDTO>> response = _itemService.GetAll("Images");
            return response.Data.ToList();
        }

        [HttpGet]
        [Route("api/items/filter")]
        public List<ItemDTO> Get(string name,string make,string model)
        {
            ResponsePackage<IEnumerable<ItemDTO>> response = _itemService.Filter(name,model,make,"Images");
            return response.Data.ToList();
        }

        public IActionResult MyItems(int? UserId)
        {
            if (UserId == null)
               return RedirectToAction("Login", "Account");
            ResponsePackage<IEnumerable<ItemDTO>> response = _itemService.GetByUser(UserId.Value, "Images");
            return View(response.Data);
        }

        public IActionResult WishList(int? UserId)
        {
            if (UserId == null)
                return RedirectToAction("Login", "Account");
            ResponsePackage<IEnumerable<ItemDTO>> response = _itemService.GetFollowed(UserId.Value, "Images");
            return View(response.Data);
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            if (id != null)
            {
                int itemId = id.Value;
                ResponsePackage<ItemDTO> response = _itemService.GetItem(itemId, includeProperties: "Images");
                if (response.Status == ResponseStatus.OK)
                {
                    NewItemDTO newItemDTO = new NewItemDTO()
                    {
                        ItemDTO = response.Data,
                        CategoryList = _categoryService.GetAll().Data.Select(i => new SelectListItem
                        {
                            Text = i.Category,
                            Value = i.CategoryId.ToString()
                        })
                    };
                    return View(newItemDTO);
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "There was an error while searching for item. Error:" + response.Message);
                    return View();
                }
            }
            else
                return View(new NewItemDTO()
                {
                    ItemDTO = new ItemDTO()
                    {
                        Images = new List<ImageDTO>(),
                    },
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
            string mainImage = JsonConvert.DeserializeObject<string>(dto["MainImage"]);
            if (ModelState.IsValid)
            {
                if (itemDTO.Id == 0)
                {
                    string wwwRootPath = _hostEnviroment.WebRootPath;
                    if (dto.Files != null)
                    {
                        string fileFolderName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"img\items\" + fileFolderName);
                        Directory.CreateDirectory(uploads);
                        int i = 0;
                        itemDTO.Images = new List<ImageDTO>();
                        foreach (var file in dto.Files)
                        {
                            var extension = Path.GetExtension(file.FileName);

                            using (var fileStreams = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                            {
                                file.CopyTo(fileStreams);
                                itemDTO.Images.Add(new ImageDTO { IsMainImage = file.FileName == mainImage ? true : false, Order = i++, Url = Path.Combine(uploads, file.FileName) });
                            }
                        }
                    }
                    _itemService.AddItem(itemDTO);
                }
                else
                    _itemService.UpdateItem(itemDTO);

                return Json(new { redirectToUrl = Url.Action("Index", "Item") });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var obj = _itemService.GetItem(id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            string wwwRootPath = _hostEnviroment.WebRootPath;
            var oldImagePath = Path.Combine(wwwRootPath, obj.Data.Images.First().Url);
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath.Substring(oldImagePath.IndexOf(obj.Data.Images.First().Url)));
            }

            ResponsePackage<bool> response = _itemService.DeleteItem(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            ResponsePackage<ItemDTO> response = _itemService.GetItem(id,"Images");

            return View(response.Data);
        }

        [HttpPost]
        public IActionResult Follow(int Id) {
            int? userId;
            string user;
            HttpContext.Request.Cookies.TryGetValue("LoginCookieId",out user);
            ResponsePackage<bool> response;
            if (user == null)
            {
                userId = HttpContext.Session.GetInt32("Id");
                if (userId != null)
                {
                    response = _userService.FollowItem(userId.Value, Id);
                }
                else
                    return View();
            }
            else
            {
                userId = Convert.ToInt32(user);
                response = _userService.FollowItem(userId.Value, Id);
            }
            return RedirectToAction("Details", new { id = Id });
        }

        [HttpPost]
        public IActionResult UnFollow(int Id)
        {
            int? userId;
            string user;
            HttpContext.Request.Cookies.TryGetValue("LoginCookieId", out user);
            ResponsePackage<bool> response;
            if (user == null)
            {
                userId = HttpContext.Session.GetInt32("Id");
                if (userId != null)
                {
                    response = _userService.UnFollowItem(userId.Value, Id);
                }
                else
                    return View();
            }
            else
            {
                userId = Convert.ToInt32(user);
                response = _userService.UnFollowItem(userId.Value, Id);
            }
            return RedirectToAction("Details", new { id = Id });
        }
    }
}