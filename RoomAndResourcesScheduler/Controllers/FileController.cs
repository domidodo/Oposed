using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RoomAndResourcesScheduler.Attributes;
using RoomAndResourcesScheduler.Models;
using System.Globalization;
using System.Text;
using System.Web;

namespace RoomAndResourcesScheduler.Controllers
{
    public class FileController : Controller
    {
        private const string LOGIN_URL = "/User/Login";

        [HttpGet]
        [Route("Files/Event/{eventId}/Ics/{authKey}")]
        public async Task<ActionResult> GetCalenderOfEventAsync(int eventId, string authKey)
        {
            var apiUrl = ApplicationSettings.GetConfiguration().GetValue<string>("ApiUrl");

            Event evt;
            try
            {
                User usr = await $"{apiUrl}/User/Current"
                                    .WithHeader("AuthKey", authKey)
                                    .GetJsonAsync<User>();

                evt = await $"{apiUrl}/Event/{eventId}"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<Event>();
            }
            catch (Exception)
            {
                this.HttpContext.Response.Redirect(LOGIN_URL);
                return BadRequest();
            }

            var url = Request.Scheme + "://" + Request.Host.Value + "/Event/" + evt.Id;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("METHOD:PUBLISH");

            sb.AppendLine("BEGIN:VTIMEZONE");
            sb.AppendLine("TZID:Europe/Berlin");
            sb.AppendLine("END:VTIMEZONE");

            
            foreach (var time in evt.Schedule) {
                sb.AppendLine("BEGIN:VEVENT");
                sb.AppendLine("LOCATION:"+evt.Resource.Name);
                sb.AppendLine("SUMMARY:"+evt.Name);
                sb.AppendLine("DESCRIPTION:"+evt.Description + "<br/><br/><br/><a href='mailto:" + evt.Organizer.Mail + "'>" + evt.Organizer.Mail+ "</a><br/><br/><a href='" + url + "'>" + url+"</a>");
                sb.AppendLine("DTSTART:"+ time.From.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo) +"T"+ time.From.ToString("HHmmss", DateTimeFormatInfo.InvariantInfo) + "Z");
                sb.AppendLine("DTEND:" + time.From.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo) + "T" + time.From.ToString("HHmmss", DateTimeFormatInfo.InvariantInfo) + "Z");
                sb.AppendLine("END:VEVENT");
            }
           
            sb.AppendLine("END:VCALENDAR");

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write(sb.ToString());
            writer.Flush();
            stream.Position = 0;

            return File(stream, "text/calendar", "Event.ics");
        }
    }
}
