using System.Linq;
using BlogApp.Entity;

namespace BlogApp.WebUI
{
    public class HomeBlogModel
    {
        public IOrderedQueryable<Blog> HomeBlogs { get; set; }
        public IOrderedQueryable<Blog> SliderBlogs { get; set; }
    }
}