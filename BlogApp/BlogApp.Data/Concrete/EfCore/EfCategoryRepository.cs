using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfCategoryRepository:ICategoryRepository
    {
        private BlogContext _context;

        public EfCategoryRepository(BlogContext context)
        {
            _context = context;
        }

        public Category GetById(int categoryId)
        {
            return _context.Categories.Find(categoryId);
        }

        public IQueryable<Category> GetAll()
        {
            return _context.Categories.Include(i=>i.Blogs);
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void SaveCategory(Category category)
        {
            if (category.CategoryId==0)
            {
                _context.Categories.Add(category);
            }
            else
            {
                var entity = GetById(category.CategoryId);
                if (category!=null)
                {
                    entity.Name = category.Name;
                   
                }
            }
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void DeleteCategory(int categoryId)
        {
            _context.Categories.Remove(GetById(categoryId));
            _context.SaveChanges();
        }

    }
}
