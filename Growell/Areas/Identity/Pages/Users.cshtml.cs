using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace Growell.Areas.Identity.Pages
{
    [Authorize]
    public class UsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        public ApplicationUser? appUser;
        public UsersModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async void OnGet()
        {
            var task = userManager.GetUserAsync(User);
           task.Wait();
            appUser = task.Result;
        }
    }
}
