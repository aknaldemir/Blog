using System.Linq;
using BlogApp.Entity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BlogApp.WebUI
{
    public class BlogModel
    { 
        public IQueryable<Blog> Blogs { get; set; }
       
    }
}