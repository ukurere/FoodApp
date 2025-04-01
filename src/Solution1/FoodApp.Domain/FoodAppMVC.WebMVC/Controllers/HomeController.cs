using Microsoft.AspNetCore.Mvc;

namespace FoodAppMVC.WebMVC.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
