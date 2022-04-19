using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;

namespace Oposed.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Login()
        {
            Microsoft.Extensions.Primitives.StringValues lang;

            if (HttpContext.Request.Query.TryGetValue("lng", out lang))
            {
                var langStr = lang.FirstOrDefault();
                if (langStr != null)
                {
                    Response.Cookies.Append(
                      CookieRequestCultureProvider.DefaultCookieName,
                      CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(langStr)),
                      new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                  );
                }
                Response.Redirect(HttpContext.Request.Path);
            }

            return View();
        }

    }
}
