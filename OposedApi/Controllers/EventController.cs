using Microsoft.AspNetCore.Mvc;
using OposedApi.Attributes;
using OposedApi.Enum;
using OposedApi.Error;
using OposedApi.Models;
using OposedApi.Utilities;
using Swashbuckle.AspNetCore.Annotations;

namespace OposedApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        [HttpGet]
        [Auth(Role = UserRole.User)]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get event")]
        public ActionResult<Event> GetEventById(int id)
        {
            var evt = EventUtility.GetEventById(id);
            if (evt != null)
                return evt;
            else
                return ErrorManager.Get(Errors.EVENT_NOT_FOUND);
        }

        [HttpGet]
        [Auth(Role = UserRole.User)]
        [SwaggerOperation(Summary = "Get all current and future events")]
        public ActionResult<List<Event>> GetAllRoomEvents()
        {
            return EventUtility.GetAllEvents();
        }

        [HttpGet]
        [Auth(Role = UserRole.User)]
        [Route("Room/{roomId}")]
        [SwaggerOperation(Summary = "Get all current and future events of room")]
        public ActionResult<List<Event>> GetAllCurrentFutureEventsByRoomId(int roomId)
        {
            return EventUtility.GetAllEventsOfRoom(roomId);
        }

        [HttpGet]
        [Auth(Role = UserRole.User)]
        [Route("Room/{roomId}/All")]
        [SwaggerOperation(Summary = "Get all current, future and past events of room")]
        public ActionResult<List<Event>> GetAllEventsByRoomId(int roomId)
        {
            return EventUtility.GetAllEventsOfRoom(roomId, true);
        }

        [HttpGet]
        [Auth(Role = UserRole.User)]
        [Route("Room/{roomId}/Next")]
        [SwaggerOperation(Summary = "Get current or next events of room")]
        public ActionResult<Event> GetNextEventsByRoomId(int roomId)
        {
            var evt = EventUtility.GetNextEventsOfRoom(roomId);
            if (evt != null)
                return evt;
            else
                return ErrorManager.Get(Errors.EVENT_NOT_FOUND);
        }

        [HttpGet]
        [Auth(Role = UserRole.User)]
        [Route("Room/{roomId}/BlockedTimePeriod")]
        [SwaggerOperation(Summary = "Check if room is free. Entity in rentun are blocked.")]
        public ActionResult<List<TimePeriod>> GetBlockedTimePeriod(int roomId, List<TimePeriod> time)
        {
            return EventUtility.GetBlockedTimePeriods(roomId, time);
        }

        [HttpPost]
        [Auth(Role = UserRole.User)]
        [SwaggerOperation(Summary = "Add event")]
        public ActionResult<Event> AddEvent(Event eve)
        {
            var organizer = UserUtility.GetUser(eve.OrganizerId);
            var currentUser = UserUtility.GetCurrentUser(HttpContext);
            if (organizer == null)
            {
                return ErrorManager.Get(Errors.USER_NOT_FOUND);
            }

            if (currentUser.Role == UserRole.User && currentUser.Id != eve.OrganizerId)
            {
                return ErrorManager.Get(Errors.PERMISSIONS_FAILED);
            }

            var newEvent = EventUtility.AddEvent(eve);
            if (newEvent != null)
                return newEvent;
            else
                return ErrorManager.Get(Errors.EVENT_INSERT_FAILED);
        }

        [HttpPut]
        [Auth(Role = UserRole.User)]
        [SwaggerOperation(Summary = "Update event")]
        public ActionResult<Event> UpdateEvent(Event eve)
        {
            var organizer = UserUtility.GetUser(eve.OrganizerId);
            var currentUser = UserUtility.GetCurrentUser(HttpContext);
            if (organizer == null)
            {
                return ErrorManager.Get(Errors.USER_NOT_FOUND);
            }

            if (currentUser.Role == UserRole.User && currentUser.Id != eve.OrganizerId) 
            {
                return ErrorManager.Get(Errors.PERMISSIONS_FAILED);
            }

            var successful = EventUtility.UpdateEvent(eve);
            if (successful)
                return Ok();
            else
                return ErrorManager.Get(Errors.EVENT_UPDATING_FAILED);
        }

        [HttpPut]
        [Auth(Role = UserRole.User)]
        [SwaggerOperation(Summary = "Join current user as visitor to event")]
        [Route("{eventId}/Join")]
        public ActionResult<Event> JoinToEvent(int eventId)
        {
            var currentUser = UserUtility.GetCurrentUser(HttpContext);

            var evt = EventUtility.GetEventById(eventId);
            evt.VisitorIds.Add(currentUser.Id);

            var successful = EventUtility.UpdateEvent(evt);
            if (successful)
                return Ok();
            else
                return ErrorManager.Get(Errors.EVENT_UPDATING_FAILED);
        }

        [HttpPut]
        [Auth(Role = UserRole.User)]
        [SwaggerOperation(Summary = "Unjoin current user from visitor of event")]
        [Route("{eventId}/Unjoin")]
        public ActionResult<Event> UnjoinToEvent(int eventId)
        {
            var currentUser = UserUtility.GetCurrentUser(HttpContext);

            var evt = EventUtility.GetEventById(eventId);
            evt.VisitorIds.Remove(currentUser.Id);

            var successful = EventUtility.UpdateEvent(evt);
            if (successful)
                return Ok();
            else
                return ErrorManager.Get(Errors.EVENT_UPDATING_FAILED);
        }

        [HttpDelete]
        [Auth(Role = UserRole.User)]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Delete event")]
        public ActionResult DeleteEventsToRoom(int id)
        {
            var successful = EventUtility.DeleteEventById(id);
            if (successful)
                return Ok();
            else
                return ErrorManager.Get(Errors.EVENT_DELETING_FAILED);
        }
    }
}
