namespace StringApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            return this.View();
        }

        public IActionResult Index()
        {
            return this.View();
        }
    }
}