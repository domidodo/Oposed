using OposedApi.Enum;
using OposedApi.MailType;
using System.Net;
using System.Net.Mail;

namespace OposedApi.Utilities
{
    internal class MailSenderUtility
    {
        private static SmtpClient? _smtpClient = null;

        private static void Init()
        {
            var host = Settings.SmtpServerHost;
            var port = Convert.ToInt32(Settings.SmtpServerPort);
            var mail = Settings.SmtpServerMailAddress;
            var password = Settings.SmtpServerMailPassword;
            var ignoreInvalidCertificate = Convert.ToBoolean(Settings.SmtpServerIgnoreInvalidCertificate);
            var ssl = Convert.ToBoolean(Settings.SmtpServerIsSsl);

            if (host == null)
                return;

            _smtpClient = new SmtpClient(host)
            {
                Port = port,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mail, password),
                EnableSsl = ssl,
            };
            if (ignoreInvalidCertificate)
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            }
        }

        internal static void Send(MailTypBase type)
        {
            if (_smtpClient == null)
                Init();
            if (_smtpClient == null)
                return;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(Settings.SmtpServerMailAddress),
                Subject = type.GetSubject(),
                Body = type.GetHtmlContent(),
                IsBodyHtml = true,
            };
            foreach (var mail in type.GetMailList())
            {
                mailMessage.To.Add(mail);
            }

            _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
