using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Oposed.Attributes;
using Oposed.Models;
using System.Diagnostics;

namespace Oposed.Controllers
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
                User usr = GetUser(HttpContext);
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
                User usr = GetUser(HttpContext);
                vm.Resource = await $"{apiUrl}/Resource/{resourceId}"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<Resource>();
                var eventList = await $"{apiUrl}/Event/Resource/{resourceId}"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<List<Event>>();
                vm.EventWithFrom = ToEventWithSchedule(eventList, true);

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
                User usr = GetUser(HttpContext);
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
                User usr = GetUser(HttpContext);

                evt = await $"{apiUrl}/Event/{eventId}"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<Event>();
                
                if (evt.Organizer != null)
                {
                    evt.Organizer.OrganizedEventWithSchedule = ToEventWithSchedule(evt.Organizer.OrganizedEvents, false, true);
                }
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
        public async Task<IActionResult> NewEvent()
        {
            User usr = GetUser(HttpContext);
            var apiUrl = Settings.UrlApi;
            var vm = new EventViewModel();

            if (Request.Query.TryGetValue("ResourceId", out var resourceId))
            {
                vm.Event.RoomId = Int32.Parse(resourceId);
            }

            if (Request.Query.TryGetValue("isPrivate", out var isPrivate))
            {
                vm.Event.IsPrivate = bool.Parse(isPrivate);
            }

            if (Request.Query.TryGetValue("joinNotification", out var joinNotification))
            {
                vm.Event.EnableJoinNotification = bool.Parse(joinNotification);
            }

            if (Request.Query.TryGetValue("Name", out var name))
            {
                vm.Event.Name = name;
            }

            if (Request.Query.TryGetValue("Description", out var description))
            {
                vm.Event.Description = description;
            }

            if (Request.Query.TryGetValue("VisitorIds", out var visitorIds))
            {
                var visitorIdsList = visitorIds.ToString().Split(',');
                vm.Event.VisitorIds = visitorIdsList.Select(x => Int32.Parse(x)).ToList();
            }

            if (Request.Query.TryGetValue("MaxVisitorCount", out var maxVisitorCount))
            {
                vm.Event.MaxVisitorCount = Int32.Parse(maxVisitorCount);
            }

            if (Request.Query.TryGetValue("Tags", out var tags))
            {
                var tagsList = visitorIds.ToString().Split(',');
                vm.Event.Tags = tagsList.ToList();
            }

            try
            {
                vm.Templates = await $"{apiUrl}/Template"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<List<Template>>();
            }
            catch (Exception){}

            try
            {
                vm.Resources = await $"{apiUrl}/Resource"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<List<Resource>>();
            }
            catch (Exception) { }

            try
            {
                vm.Tags = await $"{apiUrl}/Newsletter/tags"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<List<string>>();
            }
            catch (Exception) { }


            return View("EventForm", vm);
        }

        [Auth]
        [Route("Event/{eventId}/Edit")]
        public async Task<IActionResult?> EditEvent(int  eventId)
        {
            var apiUrl = Settings.UrlApi;
            User usr = GetUser(HttpContext);
            var vm = new EventViewModel();

            try
            {
                vm.Event = await $"{apiUrl}/Event/{eventId}"
                        .WithHeader("AuthKey", usr.AuthKey)
                        .GetJsonAsync<Event>();
            }
            catch (Exception)
            {
                HttpContext.Response.Redirect(LOGIN_URL);
                return null;
            }

            try
            {
                vm.Templates = await $"{apiUrl}/Template"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<List<Template>>();
            }
            catch (Exception) { }

            try
            {
                vm.Resources = await $"{apiUrl}/Resource"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<List<Resource>>();
            }
            catch (Exception) { }

            try
            {
                vm.Tags = await $"{apiUrl}/Newsletter/tags"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<List<string>>();
            }
            catch (Exception) { }

            return View("EventForm", vm);
        }

        [Auth]
        [Route("Events")]
        public async Task<IActionResult?> AllEvents()
        {
            var apiUrl = Settings.UrlApi;
            User usr = GetUser(HttpContext);
            var vm = new List<EventWithSchedule>();

            try
            {
                var evt = await $"{apiUrl}/Event/"
                        .WithHeader("AuthKey", usr.AuthKey)
                        .GetJsonAsync<List<Event>>();
                vm = ToEventWithSchedule(evt);
            }
            catch (Exception)
            {
                HttpContext.Response.Redirect(LOGIN_URL);
                return null;
            }

            return View("EventList", vm);
        }

        [Auth]
        [Route("Borrow/New")]
        public async Task<IActionResult> NewBorrow()
        {
            User usr = GetUser(HttpContext);
            var apiUrl = Settings.UrlApi;
            var vm = new BorrowViewModel();

            if (Request.Query.TryGetValue("ResourceId", out var resourceId))
            {
                vm.DeviceId = Int32.Parse(resourceId);
            }

            try
            {
                vm.Resources = await $"{apiUrl}/Resource"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<List<Resource>>();
            }
            catch (Exception) { }

            return View("BorrowForm", vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        private User GetUser(HttpContext context) 
        {
            return context.Items["User"] as User;
        }

        private List<EventWithSchedule> ToEventWithSchedule(List<Event> eventList, bool showPrivateEvents = false, bool showPastEvents = false) 
        {
            DateTime now = DateTime.Now;
            var list = new List<EventWithSchedule>();
            foreach (var evt in eventList)
            {
                if (showPrivateEvents || !evt.IsPrivate)
                {
                    foreach (var schedule in evt.Schedule)
                    {
                        if (showPastEvents || schedule.To > now)
                        {
                            var eventWithSchedule = new EventWithSchedule()
                            {
                                Event = evt,
                                Schedule = schedule
                            };
                            list.Add(eventWithSchedule);
                        }
                    }
                }
            }

            return list.OrderBy(o => o.Schedule.From).ToList();
        }
    }
}