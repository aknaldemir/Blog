using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity.Identity;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.WebUI.Models.IdentityModels
{

    //AdminRole Edit Get metodu için
    public class RoleDetails
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<ApplicationUser> Members { get; set; }
        public IEnumerable<ApplicationUser> NonMembers { get; set; }
    }



    //AdminRole Edit Get metodu için
    public class RoleEditModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }

    }
}
