using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BlogApp.Entity.TagHelpers
{

    //Summary: identity-role adında taghelper oluşturuyoruz ve bu taghelper role listeleme
    //sayfasında rollere ait userları getiriyor.



    //Taghelper'in çalışma alanı. identity-role taghelper'i td içerisinde iken aktif olsun. 
    [HtmlTargetElement("td",Attributes = "identity-role")]
    public class RoleUserTagHelper:TagHelper
    {
        //role ait userları getirmek için gerekli 
        private UserManager<ApplicationUser> _userManager;

        //taghelperdan gelen roleid ile ilgili role almak için
        private RoleManager<IdentityRole> _roleManager;

        public RoleUserTagHelper(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        //view üzerindeki tag helper dan gelen value değerini Role property e atamak için..
        [HtmlAttributeName("identity-role")]
        public string Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            //role ait userları tutmak için list
             List<string> names=new List<string>();

             //taghelper dan gelen role id ile rolemanager üzerinden arama yaptık.
             var role = await _roleManager.FindByIdAsync(Role);

             if (role!=null)
             {
                 foreach (var user in _userManager.Users)
                 {
                     if (user!=null && await _userManager.IsInRoleAsync(user,role.Name))
                     {
                          names.Add(user.UserName);
                     }
                 }
             }

             output.Content.SetContent(names.Count == 0 ? "No User" : string.Join(", ", names));
        }

        //oluşturduğumuz taghelperi aktif etmek için ViewImport dosyasına 
        //@addTagHelper BlogApp.Entity.TagHelpers.*,BlogApp.Entity
        //taghelperin namespace'i-------------------dll adı(proje adı)
    }
}
