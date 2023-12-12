using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoeStore.Extension;
using ShoeStore.ModelViews;

namespace ShoeStore.Controllers.Components
{
    public class NumberCartViewComponent : ViewComponent
    {
        
            public IViewComponentResult Invoke()
            {
                var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
                
            return View(cart);
            }
        
        
    }
}
