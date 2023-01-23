using OposedApi.Models;
using System.Text;

namespace OposedApi.MailType
{
    internal class EditEvent : MailTypBase
    {
        private User _user;
        private Event _evt;

        internal EditEvent(User usr, Event evt) {
            _user = usr;
            _evt = evt;
        }

        internal override string GetSubject(User receiver)
        {
            var sb = new StringBuilder();
            sb.Append(_evt.Name);
            switch (receiver.Language)
            {
                case "de":
                    sb.Append(" wurde bearbeitet");
                    break;
                default: // en
                    sb.Append(" was edited");
                    break;
            }
            return sb.ToString();
        }

        protected override string BildContent(User receiver)
        {
            var description = "The event <b>" + _evt.Name + "</b>. Please check what has changed.";
            var buttonText = "To the event page";
            switch (receiver.Language)
            {
                case "de":
                    description = "Das Event <b>" + _evt.Name + "</b> wurde berarbeitet. &Uuml;berpr&uuml;fen Sie bitte was sich ge&auml;ndert hat.";
                    buttonText = "Zur Event-Seite";
                    break;
            }

            var sb = new StringBuilder();
            sb.Append("<table role=\"presentation\" style=\"width:100%;border-collapse:collapse;border:0;border-spacing:0;\">");
            sb.Append("<tr>");
            sb.Append("<td style=\"padding:0 0 36px 0;color:#153643;\">");
            sb.Append("<h1 style=\"font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;\">"+ GetSubject(receiver) + "</h1>");
            sb.Append("<p style=\"margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\">" + description + "</p>");
            sb.Append("<p style=\"margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\"><a href=\""+_baseUrl+"/Event/"+_evt.Id+"/\" style=\"color:#ee4c50;text-decoration:underline;\">"+ buttonText + "</a></p>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");

            return sb.ToString();
        }
    }
}
