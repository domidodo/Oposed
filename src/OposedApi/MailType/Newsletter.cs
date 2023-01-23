using OposedApi.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace OposedApi.MailType
{
    internal class Newsletter : MailTypBase
    {
        private List<Event> _events = new List<Event>();

        internal Newsletter(List<Event> evtList) 
        {
            _events = evtList;
        }

        protected override string BildContent(User receiver)
        {
            var buttonText = "To the event page";
            var description = "These events might interest you:";
            var freePlaces = " freie Plätze";
            switch (receiver.Language)
            {
                case "de":
                    buttonText = "Zur Event-Seite";
                    description = "Diese Events könnten dich interessieren:";
                    freePlaces = " free places";
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

                sb.Append("<td style=\"width:260px;padding:0;vertical-align:top;color:#153643;\"><p style=\"margin:0 0 25px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\"><img src=\""+evt.Image+"\" alt=\"\" width=\"260\" style=\"height:auto;display:block;\" /></p><p style=\"margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\"><b>" + evt.Name + "</b><br/>" + GetDescription(evt.Description) + "<br/><small>("+ (evt.MaxVisitorCount - evt.VisitorIds.Count) + freePlaces + ")</small></p><p style=\"margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\"><a href=\"" + _baseUrl + "/Event/" + evt.Id + "\" style=\"color:#ee4c50;text-decoration:underline;\">" + buttonText+"</a></p></td>");

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

        private string GetDescription(string description)
        {
            if (description == null)
            {
                return "";
            }
            description = Regex.Replace(description, "<.*?>", " ");
            if (description.Length > 200)
            {
                return description.Substring(0, 200);
            }
            return description;
        }

        internal override string GetSubject(User receiver)
        {
            var sb = new StringBuilder();
            switch (receiver.Language)
            {
                case "de":
                    if (_events.Count == 1)
                    {
                        sb.Append("Ein interessantes Event findet im laufe des nächsten Monats statt");
                    }
                    else 
                    {
                        sb.AppendFormat("{0} interessante Events finden nächstn Monat statt", _events.Count);
                    }
                    break;
                default: // en
                    if (_events.Count == 1)
                    {
                        sb.Append("An interesting event is taking place over the next month");
                    }
                    else
                    {
                        sb.AppendFormat("{0} interesting events will take place next month", _events.Count);
                    }
                    break;
            }
            return sb.ToString();
        }
    }
}
