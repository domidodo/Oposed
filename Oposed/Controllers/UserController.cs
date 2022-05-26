using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Oposed.Attributes;
using Oposed.Enum;
using Oposed.Models;
using Flurl.Http;

namespace Oposed.Controllers
{
    public class UserController : Controller
    {
        [Route("/User/Login")]
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

        [Auth]
        [Route("/Users")]
        public async Task<IActionResult?> ShowAllUsers()
        {
            var apiUrl = Settings.UrlApi;

            List<User> users = new List<User>();
            try
            {
                User usr = GetUser(HttpContext);

                users = await $"{apiUrl}/User/"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<List<User>>();
            }
            catch (Exception)
            {
                HttpContext.Response.Redirect("/");
                return null;
            }

            return View("UserList", users);
        }

        [Auth]
        [Route("/Tags")]
        public async Task<IActionResult?> ShowAllTags()
        {
            var apiUrl = Settings.UrlApi;

            List<Newsletter> newsletter = new List<Newsletter>();
            try
            {
                User usr = GetUser(HttpContext);

                newsletter = await $"{apiUrl}/Newsletter/"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<List<Newsletter>>();
            }
            catch (Exception)
            {
                HttpContext.Response.Redirect("/");
                return null;
            }

            return View("TagManager", newsletter);
        }


        private User GetUser(HttpContext context)
        {
            return context.Items["User"] as User;
        }

    }
}
