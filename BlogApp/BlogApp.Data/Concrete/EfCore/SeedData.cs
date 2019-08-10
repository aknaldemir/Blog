using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlogApp.Data.Concrete.Identity;
using BlogApp.Entity;
using BlogApp.Entity.Identity;
using BlogApp.Entity.Identity.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {  
        public static async Task Seed(IApplicationBuilder app)
        {
            var blogContext = app.ApplicationServices.GetRequiredService<BlogContext>();
           

            blogContext.Database.Migrate();
                

            if (!blogContext.Categories.Any())
            {
                blogContext.Categories.AddRange(
                    new Category() { Name = "Category 1" },
                    new Category() { Name = "Category 2" },
                    new Category() { Name = "Category 3" }
                );

                blogContext.SaveChanges();
            }             

            if (!blogContext.Blogs.Any())
            {
                blogContext.Blogs.AddRange(

                    new Blog() { Title = "Blog title 1", Description = "Blog Description", Body = "Blog Body 1", Image = "1.jpg", DateTime = DateTime.Now.AddDays(-5), IsApproved = true, CategoryId = 1 },
                    new Blog() { Title = "Blog title 2", Description = "Blog Description", Body = "Blog Body 1", Image = "2.jpg", DateTime = DateTime.Now.AddDays(-7), IsApproved = true, CategoryId = 1 },
                    new Blog() { Title = "Blog title 3", Description = "Blog Description", Body = "Blog Body 1", Image = "3.jpg", DateTime = DateTime.Now.AddDays(-8), IsApproved = false, CategoryId = 2 },
                    new Blog() { Title = "Blog title 4", Description = "Blog Description", Body = "Blog Body 1", Image = "4.jpg", DateTime = DateTime.Now.AddDays(-9), IsApproved = true, CategoryId = 3 }
                );

                blogContext.SaveChanges();
            }

           
          
        }
    }
}
