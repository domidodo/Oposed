using OposedApi.Models;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OposedApi.MailType
{
    internal class Reminder : MailTypBase
    {
        private List<Event> _events = new List<Event>();

        internal Reminder(List<Event> evtList) 
        {
            _events = evtList;
        }

        protected override string BildContent(User receiver)
        {
            var buttonText = "To the event page";
            var description = "You signed up for these tomorrow's events:";
            var culture = "en-US";
            switch (receiver.Language)
            {
                case "de":
                    buttonText = "Zur Event-Seite";
                    description = "Für diese morgigen Events haben Sie sich eingeschrieben:";
                    culture = "de-DE";
                    break;
            }

            var sb = new StringBuilder();
            sb.Append("<table role=\"presentation\" style=\"width:100%;border-collapse:collapse;border:0;border-spacing:0;\">");
            sb.Append("<tr>");
            sb.Append("<td style=\"padding:0 0 36px 0;color:#153643;\">");
            sb.Append("<h1 style=\"font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;\">" + GetSubject(receiver) + "</h1>");
            sb.Append("<p style=\"margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\">" + description + "</p>");
            sb.Append("</td>");
            sb.Append("</tr>");

            for (int i = 0; i < _events.Count; i++) {
                var evt = _events[i];
                if (i % 2 == 0) {
                    sb.Append("<tr><td style=\"padding:0;\"><table role=\"presentation\" style=\"width:100%;border-collapse:collapse;border:0;border-spacing:0;\"><tr>");
                }

                var date = evt.Schedule.Find(o => o.From.Date == DateTime.Today.AddDays(1)).From.ToString("g", CultureInfo.CreateSpecificCulture(culture));

                sb.Append("<td style=\"width:260px;padding:0;vertical-align:top;color:#153643;\"><p style=\"margin:0 0 25px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\"><img src=\""+evt.Image+"\" alt=\"\" width=\"260\" style=\"height:auto;display:block;\" /></p><p style=\"margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\"><b>" + evt.Name + "</b><br/>" + date + "<br/>" + evt.Room.Name + "</p><p style=\"margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\"><a href=\"" + _baseUrl + "/Event/" + evt.Id + "\" style=\"color:#ee4c50;text-decoration:underline;\">" + buttonText+"</a></p></td>");

                if (i % 2 == 0)
                {
                    sb.Append("<td style=\"width: 20px; padding: 0; font-size:0; line-height:0;\">&nbsp;</td>");
                }
                else 
                {
                    sb.Append("</tr></table></td></tr>");
                }
            }
            if (_events.Count % 2 == 1) {
                sb.Append("<td style=\"width:260px;padding:0;vertical-align:top;\"></td>");
                sb.Append("</tr></table></td></tr>");
            }

            sb.Append("</table>");

            return sb.ToString();
        }

        internal override string GetSubject(User receiver)
        {
            var sb = new StringBuilder();
            switch (receiver.Language)
            {
                case "de":
                    if (_events.Count == 1)
                    {
                        sb.Append("Ein Event findet");
                    }
                    else 
                    {
                        sb.AppendFormat("{0} Events finden", _events.Count);
                    }
                    sb.Append(" morgen statt");
                    break;
                default: // en
                    if (_events.Count == 1)
                    {
                        sb.Append("An event is taking");
                    }
                    else
                    {
                        sb.AppendFormat("{0} events will take", _events.Count);
                    }
                    sb.Append(" place tomorrow");
                    break;
            }
            return sb.ToString();
        }
    }
}
