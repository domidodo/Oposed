using OposedApi.Models;
using System.Text;

namespace OposedApi.MailType
{
    internal class Join : MailTypBase
    {
        private User _user;
        private Event _evt;

        internal Join(User usr, Event evt) {
            _user = usr;
            _evt = evt;
        }

        internal override string GetSubject(string lang)
        {
            var surname = _user.Surname;
            if (surname.Length > 1) {
                surname = surname.Substring(0, 1);
            }
            var sb = new StringBuilder();
            sb.Append(_user.Name);
            sb.Append(" ");
            sb.Append(surname);
            switch (lang)
            {
                case "de":
                    sb.Append(" ist beigetreten");
                    break;
                default: // en
                    sb.Append(" joined");
                    break;
            }
            return sb.ToString();
        }

        protected override string BildContent(string lang)
        {
            var description = _user.Name + " " + _user.Surname + " signed up for your event <b>" + _evt.Name + "</b>. There are now " + (_evt.MaxVisitorCount - _evt.VisitorIds.Count) + " spaces available.";
            var buttonText = "To the event page";
            switch (lang)
            {
                case "de":
                    description = _user.Name + " " + _user.Surname + " hat sich in Ihr Event <b>" + _evt.Name + "</b> am eingeschrieben. Damit sind nur noch " + (_evt.MaxVisitorCount - _evt.VisitorIds.Count) + " Pl&auml;tze frei.";
                    buttonText = "Zur Event-Seite";
                    break;
            }

            var sb = new StringBuilder();
            sb.Append("<table role=\"presentation\" style=\"width:100%;border-collapse:collapse;border:0;border-spacing:0;\">");
            sb.Append("<tr>");
            sb.Append("<td style=\"padding:0 0 36px 0;color:#153643;\">");
            sb.Append("<h1 style=\"font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;\">"+ GetSubject(lang) + "</h1>");
            sb.Append("<p style=\"margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\">" + description + "</p>");
            sb.Append("<p style=\"margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\"><a href=\""+_baseUrl+"/Event/"+_evt.Id+"/\" style=\"color:#ee4c50;text-decoration:underline;\">"+ buttonText + "</a></p>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");

            return sb.ToString();
        }
    }
}
