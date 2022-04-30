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

            List<Room> rooms = new List<Room>();

            try
            {
                User usr = GetUser(HttpContext);
                rooms = await $"{apiUrl}/Room"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<List<Room>>();
            }
            catch (Exception)
            {
                HttpContext.Response.Redirect(LOGIN_URL);
                return null;
            }

            return View(rooms);
        }

        [Auth]
        [Route("Room/{roomId}")]
        public async Task<IActionResult?> Room(int roomId)
        {
             var apiUrl = Settings.UrlApi;

           RoomEventViewModel vm = new RoomEventViewModel();

            try
            {
                User usr = GetUser(HttpContext);
                vm.Room = await $"{apiUrl}/Room/{roomId}"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<Room>();
                var eventList = await $"{apiUrl}/Event/Room/{roomId}"
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
        [Route("Room/New")]
        public IActionResult NewRoom()
        {
            var vm = new Room();

            if (Request.Query.TryGetValue("Name", out var name))
            {
                vm.Name = name;
            }

            if (Request.Query.TryGetValue("Description", out var description))
            {
                vm.Description = description;
            }

            return View("RoomForm", vm);
        }

        [Auth]
        [Route("Room/{roomId}/Edit")]
        public async Task<IActionResult?> EditRoom(int roomId)
        {
            var apiUrl = Settings.UrlApi;

            var vm = new Room();

            try
            {
                User usr = GetUser(HttpContext);
                vm = await $"{apiUrl}/Room/{roomId}"
                        .WithHeader("AuthKey", usr.AuthKey)
                        .GetJsonAsync<Room>();
            }
            catch (Exception)
            {
                HttpContext.Response.Redirect(LOGIN_URL);
                return null;
            }

            return View("RoomForm", vm);
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

            if (Request.Query.TryGetValue("RoomId", out var roomId))
            {
                vm.Event.RoomId = Int32.Parse(roomId);
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
                vm.Rooms = await $"{apiUrl}/Room"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<List<Room>>();
            }
            catch (Exception) { }


            vm.Tags = GetTags();


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
                vm.Rooms = await $"{apiUrl}/Room"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<List<Room>>();
            }
            catch (Exception) { }

            vm.Tags = GetTags();


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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        private User GetUser(HttpContext context) 
        {
            return context.Items["User"] as User;
        }

        private List<EventWithSchedule> ToEventWithSchedule(List<Event> eventList, bool addPrivateEvents = false) 
        {
            DateTime now = DateTime.Now;
            var list = new List<EventWithSchedule>();
            foreach (var evt in eventList)
            {
                if (addPrivateEvents || !evt.IsPrivate)
                {
                    foreach (var schedule in evt.Schedule)
                    {
                        if (schedule.From > now)
                        {
                            list.Add(new EventWithSchedule()
                            {
                                Event = evt,
                                Schedule = schedule
                            });
                        }
                    }
                }
            }

            return list.OrderBy(o => o.Schedule.From).ToList();
        }

        private List<String> GetTags() 
        {
            // TODO: Use API
            return new List<string>() { "Sipervision", "Kinder" };
        }
    }
}