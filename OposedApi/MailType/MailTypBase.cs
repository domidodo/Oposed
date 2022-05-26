using System.Text;

namespace OposedApi.MailType
{
    // https://github.com/tutsplus/build-an-html-email-template-from-scratch

    internal abstract class MailTypBase
    {
        protected static string _baseUrl = Settings.BaseUrl;

        internal abstract string GetSubject();
        internal abstract List<string> GetMailList();
        protected abstract string BildContent();

        private string GetHtmlHeader() { 
            return "<!DOCTYPE html><html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\"><head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width,initial-scale=1\"><meta name=\"x-apple-disable-message-reformatting\"><title></title><!--[if mso]><noscript><xml><o:OfficeDocumentSettings><o:PixelsPerInch>96</o:PixelsPerInch></o:OfficeDocumentSettings></xml></noscript><![endif]--><style>table, td, div, h1, p {font-family: Arial, sans-serif;}</style></head><body style=\"margin:0;padding:0;\"><table role=\"presentation\" style=\"width:100%;border-collapse:collapse;border:0;border-spacing:0;background:#ffffff;\"><tr><td align=\"center\" style=\"padding:0;\"><table role=\"presentation\" style=\"width:602px;border-collapse:collapse;border:1px solid #cccccc;border-spacing:0;text-align:left;\"><tr><td align=\"center\" style=\"padding:40px 0 30px 0;background:#70bbd9;\"><img src=\""+_baseUrl+"/img/Oposed-Logo.png\" alt=\"\" width=\"300\" style=\"height:auto;display:block;\" /></td></tr><tr><td style=\"padding:36px 30px 42px 30px;\">";
        }

        private string GetHtmlFooter()
        {
            return "</td></tr><tr><td style=\"padding:30px;background:#ee4c50;\"><table role=\"presentation\" style=\"width:100%;border-collapse:collapse;border:0;border-spacing:0;font-size:9px;font-family:Arial,sans-serif;\"><tr><td style=\"padding:0;width:50%;\" align=\"left\"><p style=\"margin:0;font-size:14px;line-height:16px;font-family:Arial,sans-serif;color:#ffffff;\">&reg; Oposed 2022<br/><a href=\""+ _baseUrl + "/Notifications\" style=\"color:#ffffff;text-decoration:underline;\">Unsubscribe</a></p></td><td style=\"padding:0;width:50%;\" align=\"right\"><table role=\"presentation\" style=\"border-collapse:collapse;border:0;border-spacing:0;\"><tr><td style=\"padding:0 0 0 10px;width:150px;\"><a href=\"" + _baseUrl + "\" style=\"color:#ffffff;\">"+ _baseUrl + "</a></td></tr></table></td></tr></table></td></tr></table></td></tr></table></body></html>";
        }

        internal string GetHtmlContent()
        {
            var sb = new StringBuilder();
            sb.Append(GetHtmlHeader());
            sb.Append(BildContent());
            sb.Append(GetHtmlFooter());

            return sb.ToString();
        }
    }
}
