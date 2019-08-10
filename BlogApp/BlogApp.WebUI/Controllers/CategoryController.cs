using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebUI.Controllers
{

    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        } 
        
        public IActionResult Index()
        {
            return View(_categoryRepository.GetAll());
        }  

        public IActionResult AddOrUpdate(int? id)
        {
            if (id==null)
            {
                return View(new Category());
            }
            else
            {
                return View(_categoryRepository.GetById((int) id));
            }
        }
        [HttpPost]
        public IActionResult AddOrUpdate(Category category)
        {
            if (ModelState.IsValid)
            {
                 _categoryRepository.SaveCategory(category);
                 TempData["message"] = $"{category.Name} kayıt edildi";
                 return RedirectToAction("Index");
            }

            return View(category);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_categoryRepository.GetById(id));
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
           _categoryRepository.DeleteCategory(id);
            TempData["message"] = $"{id} numaralı kayıt silindi.";
            return RedirectToAction("Index");
        }
    }
}