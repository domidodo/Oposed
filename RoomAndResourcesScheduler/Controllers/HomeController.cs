using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using RoomAndResourcesScheduler.Attributes;
using RoomAndResourcesScheduler.Models;
using System.Diagnostics;

namespace RoomAndResourcesScheduler.Controllers
{
    public class HomeController : Controller
    {
        private const string LOGIN_URL = "/User/Login";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Auth]
        public async Task<IActionResult?> IndexAsync()
        {
            var apiUrl = Settings.UrlApi;

            List<Resource> resourcen = new List<Resource>();

            try
            {
                User usr = HttpContext.Items["User"] as User;
                resourcen = await $"{apiUrl}/Resource"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<List<Resource>>();
            }
            catch (Exception)
            {
                HttpContext.Response.Redirect(LOGIN_URL);
                return null;
            }

            return View(resourcen);
        }

        [Auth]
        [Route("Resource/{resourceId}")]
        public async Task<IActionResult?> Resource(int resourceId)
        { // /Home/Resource/1
             var apiUrl = Settings.UrlApi;

           ResourceViewModel vm = new ResourceViewModel();

            try
            {
                User usr = HttpContext.Items["User"] as User;
                vm.Resource = await $"{apiUrl}/Resource/{resourceId}"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<Resource>();
                vm.Events = await $"{apiUrl}/Event/Resource/{resourceId}"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<List<Event>>();

            }
            catch (Exception)
            {
                HttpContext.Response.Redirect(LOGIN_URL);
                return null;
            }

            return View(vm);
        }

        [Auth]
        [Route("Resource/New")]
        public IActionResult NewResource()
        {
            var vm = new Resource();

            if (Request.Query.TryGetValue("Type", out var type)) {
                if (System.Enum.TryParse(typeof(Enum.ResourceType), type.ToString(), true, out var type2)) {
#pragma warning disable CS8605
                    vm.Type = (Enum.ResourceType)type2;
#pragma warning restore CS8605
                }
            }

            if (Request.Query.TryGetValue("Name", out var name))
            {
                vm.Name = name;
            }

            if (Request.Query.TryGetValue("Description", out var description))
            {
                vm.Description = description;
            }

            return View("ResourceForm", vm);
        }

        [Auth]
        [Route("Resource/{resourceId}/Edit")]
        public async Task<IActionResult?> EditResource(int resourceId)
        {
            var apiUrl = Settings.UrlApi;

            var vm = new Resource();

            try
            {
                User usr = HttpContext.Items["User"] as User;
                vm = await $"{apiUrl}/Resource/{resourceId}"
                        .WithHeader("AuthKey", usr.AuthKey)
                        .GetJsonAsync<Resource>();
            }
            catch (Exception)
            {
                HttpContext.Response.Redirect(LOGIN_URL);
                return null;
            }

            return View("ResourceForm", vm);
        }

        [Auth]
        [Route("Event/{eventId}")]
        public async Task<IActionResult?> Event(int eventId)
        { 
            var apiUrl = Settings.UrlApi;

            Event evt;
            try
            {
                User usr = HttpContext.Items["User"] as User;

                evt = await $"{apiUrl}/Event/{eventId}"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<Event>();
            }
            catch (Exception)
            {
                HttpContext.Response.Redirect(LOGIN_URL);
                return null;
            }

            return View(evt);
        }

        [Auth]
        [Route("Event/New")]
        public IActionResult NewEvent()
        {
            var vm = new Event();

            if (Request.Query.TryGetValue("ResourceId", out var resourceId))
            {
                vm.ResourceId = Int32.Parse(resourceId);
            }

            if (Request.Query.TryGetValue("isPrivate", out var isPrivate))
            {
                vm.IsPrivate = bool.Parse(isPrivate);
            }

            if (Request.Query.TryGetValue("joinNotification", out var joinNotification))
            {
                vm.EnableJoinNotification = bool.Parse(joinNotification);
            }

            if (Request.Query.TryGetValue("Name", out var name))
            {
                vm.Name = name;
            }

            if (Request.Query.TryGetValue("Description", out var description))
            {
                vm.Description = description;
            }

            if (Request.Query.TryGetValue("VisitorIds", out var visitorIds))
            {
                var visitorIdsList = visitorIds.ToString().Split(',');
                vm.VisitorIds = visitorIdsList.Select(x => Int32.Parse(x)).ToList();
            }

            if (Request.Query.TryGetValue("MaxVisitorCount", out var maxVisitorCount))
            {
                vm.MaxVisitorCount = Int32.Parse(maxVisitorCount);
            }

            if (Request.Query.TryGetValue("Tags", out var tags))
            {
                var tagsList = visitorIds.ToString().Split(',');
                vm.Tags = tagsList.ToList();
            }

            return View("EventForm", vm);
        }

        [Auth]
        [Route("Event/{eventId}/Edit")]
        public async Task<IActionResult?> EditEvent(int  eventId)
        {
            var apiUrl = Settings.UrlApi;

            var vm = new Event();

            try
            {
                User usr = HttpContext.Items["User"] as User;
                vm = await $"{apiUrl}/Event/{eventId}"
                        .WithHeader("AuthKey", usr.AuthKey)
                        .GetJsonAsync<Event>();
            }
            catch (Exception)
            {
                HttpContext.Response.Redirect(LOGIN_URL);
                return null;
            }

            return View("EventForm", vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}