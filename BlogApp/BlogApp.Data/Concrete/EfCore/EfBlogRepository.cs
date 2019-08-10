using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfBlogRepository : IBlogRepository
    {
        private BlogContext _context;

        public EfBlogRepository(BlogContext context)
        {
            _context = context;
        }

        public void AddBlog(Blog blog)
        {
            _context.Blogs.Add(blog);
            _context.SaveChanges();
        }

        public void SaveBlog(Blog blog)
        {
            if (blog.BlogId == 0)
            {
                //create işlemi
                blog.DateTime=DateTime.Now;
                _context.Blogs.Add(blog);
            }
            else
            {
                //update işlemi
                //_context.Entry(blog).State = EntityState.Modified;
                var entity = GetById(blog.BlogId);
                if (entity!=null)
                {
                    entity.Title = blog.Title;
                    entity.Description = blog.Description;
                    entity.CategoryId = blog.CategoryId;
                    entity.Image = blog.Image;
                    entity.Body = blog.Body;
                    entity.IsApproved = blog.IsApproved;
                    entity.IsHome = blog.IsHome;
                    entity.IsSlider = blog.IsSlider;
                } 
            }
            _context.SaveChanges();
        }

        public void DeleteBlog(int blogId)
        {
            _context.Blogs.Remove(GetById(blogId));
            _context.SaveChanges();
        }

        public IQueryable<Blog> GetAll()
        {
            return _context.Blogs;
        }

        public Blog GetById(int blogId)
        {
            return _context.Blogs.Where(i=>i.BlogId==blogId)
                .Include(i=>i.Category)
                .FirstOrDefault();
        }

        public void UpdateBlog(Blog blog)
        {
           
        }
    }
}
