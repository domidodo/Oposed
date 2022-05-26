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

        internal override string GetSubject()
        {
            var surname = _user.Surname;
            if (surname.Length > 1) {
                surname = surname.Substring(0, 1);
            }
            return _user.Name + " " + surname + " joined";
        }

        internal override List<string> GetMailList()
        {
            return new List<string>() { _evt.Organizer.Mail };
        }

        protected override string BildContent()
        {
            var sb = new StringBuilder();
            sb.Append("<table role=\"presentation\" style=\"width:100%;border-collapse:collapse;border:0;border-spacing:0;\">");
            sb.Append("<tr>");
            sb.Append("<td style=\"padding:0 0 36px 0;color:#153643;\">");
            sb.Append("<h1 style=\"font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;\">"+ GetSubject() + "</h1>");
            sb.Append("<p style=\"margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\">"+_user.Name + " " + _user.Surname + " hat sich in Ihr Event <b>"+_evt.Name+"</b> am eingeschrieben. Damit sind nur noch "+(_evt.MaxVisitorCount - _evt.VisitorIds.Count) +" Pl&auml;tze frei.</p>");
            sb.Append("<p style=\"margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;\"><a href=\""+_baseUrl+"/Event/"+_evt.Id+"/\" style=\"color:#ee4c50;text-decoration:underline;\">Zur Event-Seite</a></p>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");

            return sb.ToString();
        }
    }
}
