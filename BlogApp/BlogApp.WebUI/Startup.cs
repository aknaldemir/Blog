using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Data.Concrete.Identity;
using BlogApp.Entity.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using BlogApp.Entity.Identity.Infrastructure;


namespace BlogApp.WebUI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940


        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<IBlogRepository, EfBlogRepository>();
            services.AddTransient<ICategoryRepository, EfCategoryRepository>();


            //MigrationsAssembly: Migrations'in kurulacağı projenin UI olduğunu belirttik. Belirtmez isek hata alıyoruz.
            services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<BlogContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("BlogConnection")
                        , b => b.MigrationsAssembly("BlogApp.WebUI")))
                .AddDbContext<ApplicationIdentityDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("BlogConnection")
                        , b => b.MigrationsAssembly("BlogApp.WebUI")));

            //services.ConfigureApplicationCookie(options => options.LoginPath = "Account/Login");  //varsayılan LoginPath


           /* //custom password validator ekleme
            services.AddTransient<IPasswordValidator<ApplicationUser>, CustomPasswordValidator>();
            //custom user validator ekleme
            services.AddTransient<IUserValidator<ApplicationUser>, CustomUserValidator>();*/

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 7;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;
                //options.User.AllowedUserNameCharacters = "abcde";//izin verilen karakterler

            })
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();


            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
                RequestPath = new PathString("/modules")
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    );
            });
            SeedData.Seed(app);
            SeedIdentity.Seed(app);
        }
    }
}
