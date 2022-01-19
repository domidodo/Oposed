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
            var host = Settings.SmtpServerHost;
            var port = Convert.ToInt32(Settings.SmtpServerPort);
            var mail = Settings.SmtpServerMailAddress;
            var password = Settings.SmtpServerMailPassword;
            var ssl = Convert.ToBoolean(Settings.SmtpServerIsSsl);

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
