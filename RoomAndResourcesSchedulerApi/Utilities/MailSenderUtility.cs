using RoomAndResourcesSchedulerApi.Enum;
using System.Net;
using System.Net.Mail;

namespace RoomAndResourcesSchedulerApi.Utilities
{
    public class MailSenderUtility
    {
        private static SmtpClient? _smtpClient = null;

        public static void Init()
        {
            var ldapConf = ApplicationSettings.GetConfiguration().GetSection("SmtpMail");

            var host = ldapConf.GetValue<string>("Host");
            var port = ldapConf.GetValue<int>("Port");
            var mail = ldapConf.GetValue<string>("Mail");
            var password = ldapConf.GetValue<string>("Password");
            var ssl = ldapConf.GetValue<bool>("EnableSsl");

            if (host == null)
                return;

            _smtpClient = new SmtpClient(host)
            {
                Port = port,
                Credentials = new NetworkCredential(mail, password),
                EnableSsl = ssl,
            };
        }

        public static void Send(string mail, MailTemplate template, Dictionary<string, string> param) 
        {
            if (_smtpClient == null)
                return;

            var mailMessage = new MailMessage
            {
                From = new MailAddress("email"),
                Subject = "subject",
                Body = "<h1>Hello</h1>",
                IsBodyHtml = true,
            };
            mailMessage.To.Add("recipient");

            _smtpClient.Send(mailMessage);
        }
    }
}
