using Microsoft.AspNetCore.Mvc;

namespace ShoeStore.Controllers
{
  
        public class AjaxContentController : Controller
        {
            public ActionResult HeaderCart()
            {
                return ViewComponent("HeaderCart");

            }
            public ActionResult HeaderFavourites()
            {
                return ViewComponent("NumberCart");
            }
        }
    
}
