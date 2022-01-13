using Microsoft.AspNetCore.Mvc;
using RoomAndResourcesSchedulerApi.Attributes;
using RoomAndResourcesSchedulerApi.Enum;
using RoomAndResourcesSchedulerApi.Error;
using RoomAndResourcesSchedulerApi.Models;
using RoomAndResourcesSchedulerApi.Utilities;
using Swashbuckle.AspNetCore.Annotations;

namespace RoomAndResourcesSchedulerApi.Controllers
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
        public ActionResult<List<Event>> GetAllResourceEvents()
        {
            return EventUtility.GetAllEvents();
        }

        [HttpGet]
        [Auth(Role = UserRole.User)]
        [Route("Resource/{resourceId}")]
        [SwaggerOperation(Summary = "Get all current and future events of resource")]
        public ActionResult<List<Event>> GetAllCurrentFutureEventsByResourceId(int resourceId)
        {
            return EventUtility.GetAllEventsOfResource(resourceId);
        }

        [HttpGet]
        [Auth(Role = UserRole.User)]
        [Route("Resource/{resourceId}/All")]
        [SwaggerOperation(Summary = "Get all current, future and past events of resource")]
        public ActionResult<List<Event>> GetAllEventsByResourceId(int resourceId)
        {
            return EventUtility.GetAllEventsOfResource(resourceId, true);
        }

        [HttpGet]
        [Auth(Role = UserRole.User)]
        [Route("Resource/{resourceId}/Next")]
        [SwaggerOperation(Summary = "Get current or next events of resource")]
        public ActionResult<Event> GetNextEventsByResourceId(int resourceId)
        {
            var evt = EventUtility.GetNextEventsOfResource(resourceId);
            if (evt != null)
                return evt;
            else
                return ErrorManager.Get(Errors.EVENT_NOT_FOUND);
        }

        [HttpGet]
        [Auth(Role = UserRole.User)]
        [Route("Resource/{resourceId}/BlockedTimePeriod")]
        [SwaggerOperation(Summary = "Check if resource is free. Entity in rentun are blocked.")]
        public ActionResult<List<TimePeriod>> GetBlockedTimePeriod(int resourceId, List<TimePeriod> time)
        {
            return EventUtility.GetBlockedTimePeriods(resourceId, time);
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

        [HttpDelete]
        [Auth(Role = UserRole.User)]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Delete event")]
        public ActionResult DeleteEventsToResource(int id)
        {
            var successful = EventUtility.DeleteEventById(id);
            if (successful)
                return Ok();
            else
                return ErrorManager.Get(Errors.EVENT_DELETING_FAILED);
        }
    }
}
