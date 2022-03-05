using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using WebApplication1.Models;

[assembly: OwinStartupAttribute(typeof(WebApplication1.Startup))]
namespace WebApplication1
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //اى حاجة هتكتبها في الكونفيجراشن هنا هتتنفذ عن بداية التطبيق
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreatDefaultRulesAndUsers(); //call this mehod when start ur app
        }

        public void CreatDefaultRulesAndUsers() //هكريت يوزر ورول وهسندهم لبعض
        {
            var roleMnager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userMnager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            IdentityRole role = new IdentityRole();
            if (!roleMnager.RoleExists("Admins"))
            {
                role.Name = "Admins";
                roleMnager.Create(role);  // creat and add role
                ApplicationUser user = new ApplicationUser(); // creat user
                user.UserName = "SA3d";
                user.Email = "Sa3d_SA3ied@hotmail.fr";
                var check = userMnager.Create(user, "Sa3d@2021"); //assign pw to user
                if (check.Succeeded)
                {
                    userMnager.AddToRole(user.Id, "Admins"); //assign role to user
                }
            }

        }
    }
}
