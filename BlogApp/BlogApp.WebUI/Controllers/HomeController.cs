using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace BlogApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IBlogRepository _blogRepository;
        private ICategoryRepository _categoryRepository;
        public HomeController(IBlogRepository blogRepository, ICategoryRepository categoryRepository)
        {
            _blogRepository = blogRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var model = new HomeBlogModel
            {
                HomeBlogs = _blogRepository.GetAll()
                     .Where(i => i.IsApproved && i.IsHome)
                     .OrderByDescending(i => i.DateTime),
                SliderBlogs = _blogRepository.GetAll()
                     .Where(i => i.IsApproved && i.IsSlider)
                     .OrderByDescending(i => i.DateTime)
            };
            return View(model);

        }
        public IActionResult List()
        {
            return View(_blogRepository.GetAll()
                .Where(i => i.IsApproved)
                .OrderByDescending(i => i.DateTime));
        }

        public IActionResult Details(int id)
        {
            return View(_blogRepository.GetById(id));
        }
    }
}