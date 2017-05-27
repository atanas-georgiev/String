namespace StringApp.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using StringApp.Data;
    using StringApp.Data.Models;
    using StringApp.Services.Identity.Managers;
    using StringApp.Web.Models.Account;

    [Authorize]
    public class AccountController : Controller
    {
        private IUserRoleManager<User, StringAppDbContext> userRoleManager;

        public AccountController(IUserRoleManager<User, StringAppDbContext> userRoleManager)
        {
            this.userRoleManager = userRoleManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var res = await this.userRoleManager.AddUserAsync(model.Email, model.Password);
                return this.Ok();
            }

            // If we got this far, something failed.
            return BadRequest(ModelState);
        }
    }
}
