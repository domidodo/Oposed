using OposedApi.Models;
using System.Text;

namespace OposedApi.MailType
{
    internal class CancelEvent : MailTypBase
    {
        private User _user;
        private Event _evt;

        internal CancelEvent(User usr, Event evt) {
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
                    sb.Append(" wurde abgesagt");
                    break;
                default: // en
                    sb.Append(" was cancelled");
                    break;
            }
            return sb.ToString();
        }

        protected override string BildContent(User receiver)
        {
            var description = "The event <b>" + _evt.Name + "</b> was cancelled. The following dates are free again:";
            switch (receiver.Language)
            {
                case "de":
                    description = "Das Event <b>" + _evt.Name + "</b> wurde abgesagt. Folgende Termine sind damit wieder frei:";
                    break;
            }

            var sbScheuleTable = new StringBuilder();
            sbScheuleTable.Append("<table role=\"presentation\" style=\"width:100%;border-collapse:collapse;border:0;border-spacing:0;\">");
            if (_evt.Schedule != null)
            {
                foreach (var period in _evt.Schedule)
                {
                    sbScheuleTable.Append("<tr>");
                    sbScheuleTable.Append("<td>");
                    sbScheuleTable.Append(period.From.ToString("s"));
                    sbScheuleTable.Append("</td>");
                    sbScheuleTable.Append("<td>");
                    sbScheuleTable.Append(period.To.ToString("s"));
                    sbScheuleTable.Append("</td>");
                    sbScheuleTable.Append("</td>");
                }
            }
            sbScheuleTable.Append("</table>");

            var sb = new StringBuilder();
            sb.Append("<table role=\"presentation\" style=\"width:100%;border-collapse:collapse;border:0;border-spacing:0;\">");
            sb.Append("<tr>");
            sb.Append("<td style=\"padding:0 0 36px 0;color:#153643;\">");
            sb.Append("<h1 style=\"font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;\">"+ GetSubject(receiver) + "</h1>");
            sb.Append("<p style=\"margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\">" + description + sbScheuleTable.ToString() + "</p>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");

            return sb.ToString();
        }
    }
}
