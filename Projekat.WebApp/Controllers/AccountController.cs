using Projekat.Shared.Constants;
using Projekat.Shared.DTOs;
using Projekat.DAL.Model;
using Projekat.Shared.Common;
using Projekat.BLL.Services.Interfaces;
using Projekat.BLL.Services.Implementations;

using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Collections;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Hosting;

namespace Projekat.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _hostEnviroment;
        public AccountController(IUserService userService,IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _hostEnviroment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public  IActionResult ActivateAndLogin(Guid guid)
        {
            ResponsePackage<UserDTO> response = _userService.ActivateUser(guid);

            if (response.Status == ResponseStatus.OK)
            {
                HttpContext.Session.SetString("Email",response.Data.Email);
                HttpContext.Session.SetString("FullName", response.Data.FirstName+" "+response.Data.LastName);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(String.Empty, response.Message);
                return View("Login");
            }
            
        }

        [HttpPost]
        [ActionName("Login")]        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginPOST(LoginDTO LoginDTO)
        {
            if (ModelState.IsValid)
            {
                ResponsePackage<UserDTO> response = _userService.LoginUser(LoginDTO);

                if (response.Status == ResponseStatus.OK)
                {
                    if (LoginDTO.RememberMe)
                    {
                        var cookieOptions = new CookieOptions();
                        cookieOptions.Expires = DateTime.Now.AddDays(365);
                        cookieOptions.Path = "/";
                        HttpContext.Response.Cookies.Append("LoginCookieEmail", response.Data.Email, cookieOptions);
                        HttpContext.Response.Cookies.Append("LoginCookieFullName", response.Data.FirstName + " " + response.Data.LastName, cookieOptions);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Email", response.Data.Email);
                        HttpContext.Session.SetString("FullName", response.Data.FirstName + " " + response.Data.LastName);
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    ModelState.AddModelError(String.Empty, response.Message);
                    return View("Login");
                }
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Invalid login atempt.");
                return View("Login");
            }
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterPOST(UserDTO UserDTO)
        {
            if (ModelState.IsValid)
            {
                var task = await _userService.RegisterUser(UserDTO);
                if(task.Status==ResponseStatus.OK)
                    return RedirectToAction("Index", "Home");
                else if(task.Status==ResponseStatus.InvalidEmail)
                    ModelState.AddModelError("email", task.Message);
                else if(task.Status==ResponseStatus.InvalidPhoneNo)
                    ModelState.AddModelError("phoneNumber", task.Message);
                else
                    ModelState.AddModelError(String.Empty, task.Message);
            }
            return View();

        }
        [HttpGet]
        public  IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Response.Cookies.Delete("LoginCookieFullName");
            HttpContext.Response.Cookies.Delete("LoginCookieEmail");
            return RedirectToAction("Index","Home");
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [ActionName("ForgotPassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
                var task = await _userService.ForgotPassword(email);
                if (task.Status == ResponseStatus.OK)
                    return RedirectToAction("Index", "Home");
                else if (task.Status == ResponseStatus.InvalidEmail)
                    ModelState.AddModelError(String.Empty, task.Message);
                else if (task.Status == ResponseStatus.AccountNotActivated)
                    ModelState.AddModelError(String.Empty, task.Message);
                else
                    ModelState.AddModelError(String.Empty, task.Message);
            
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(Guid guid)
        {
            PasswordResetDTO resetDTO= new PasswordResetDTO();
            resetDTO.PasswordGuid= guid;
            return View(resetDTO);
            
        }
        [HttpPost]
        [ActionName("ResetPassword")]
        public IActionResult ResetPassword(PasswordResetDTO passwordResetDTO)
        {
            if (ModelState.IsValid)
            {
                ResponsePackage<bool> response = _userService.ResetPassword(passwordResetDTO);
                if (response.Status == ResponseStatus.OK)
                {
                    //Notification that password is reset
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError(String.Empty, response.Message);
                    return View();
                }
            }
            return View();
        }

        [HttpGet]
        [ActionName("Profil")]
        public IActionResult Profil(string email)
        {
            ProfileDTO profileDTO=_userService.GetProfile(email).Data;

            return View(profileDTO);
        }

        [HttpPost]
        [ActionName("Profil")]
        public IActionResult Profil(ProfileDTO profileDTO, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnviroment.WebRootPath;
                if (file != null)
                {
                    ResponsePackage<ProfileDTO> profile = _userService.GetProfile(profileDTO.Email);
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"img\profilePictures");
                    var extension = Path.GetExtension(file.FileName);

                    if (profile.Data.ProfileUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, profile.Data.ProfileUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }


                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    profileDTO.ProfileUrl = @"\img\profilePictures\" + fileName + extension;
                }

                ResponsePackage<bool> response = _userService.UpdateProfile(profileDTO);
                if (response.Status == ResponseStatus.OK)
                {
                    //notification that password is reset
                    if (HttpContext.Request.Cookies["LoginCookieFullName"] != null) {
                        var cookieOptions = new CookieOptions();
                        cookieOptions.Expires = DateTime.Now.AddDays(365);
                        cookieOptions.Path = "/";
                        HttpContext.Response.Cookies.Append("LoginCookieFullName", profileDTO.FirstName + " " + profileDTO.LastName, cookieOptions);
                        HttpContext.Response.Cookies.Append("LoginCookieEmail", profileDTO.Email, cookieOptions);
                    }
                    if (HttpContext.Session.Get("FullName") != null)
                        HttpContext.Session.SetString("FullName", profileDTO.FirstName + " " + profileDTO.LastName);


                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    return View();
                }
            }
            return View();
        }


    }
}
