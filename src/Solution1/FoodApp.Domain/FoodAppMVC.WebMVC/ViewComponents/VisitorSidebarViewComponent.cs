using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace FoodAppMVC.WebMVC.ViewComponents
{
    public class VisitorSidebarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("VisitorSidebar");
        }
    }
}
