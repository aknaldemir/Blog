using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using BlogApp.Entity;
using BlogApp.Entity.Identity;
using BlogApp.WebUI.Models.IdentityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private UserManager<ApplicationUser> _userManager;

        private IPasswordValidator<ApplicationUser> _passwordValidator;
        private IPasswordHasher<ApplicationUser> _passwordHasher;

        public AdminController(UserManager<ApplicationUser> userManager, IPasswordValidator<ApplicationUser> passwordValidator, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _userManager = userManager;
            //Password güncellemesinde yazılan password ü hasleyip o şekilde kayıt etmeliyiz. Create işleminde bu bizim adımıza yapılıyor
            _passwordValidator = passwordValidator;
            _passwordHasher = passwordHasher;
        }

        public IActionResult Index() => View(_userManager.Users);

        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(RegisterModel model)
        {
            //User Create
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }



            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            //usermanager içerisindeki metotlar async olduğu için metodu async olarak tanımlıyoruz. 
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        //key olmayan mesaj model seviyesinde mesajdır. 
                        //viewde asp-validation-summary="ModelOnly" kullanabiliriz.
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }
            else
            {
                ModelState.AddModelError("", "User not Found");
            }

            return View("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Update(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(string Id, string Password, string Email)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                //update
                user.Email = Email;
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(Password))
                {
                    validPass = await _passwordValidator.ValidateAsync(_userManager, user, Password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, Password);
                    }
                    else
                    {
                        foreach (var error in validPass.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("","Parola boş bırakılamaz");
                    return View(user);
                }

                if (validPass.Succeeded)
                {
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }


            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }

            return View(user);
        }

    }

}