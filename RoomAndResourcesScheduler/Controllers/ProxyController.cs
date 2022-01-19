using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Text;

namespace RoomAndResourcesScheduler.Controllers
{
    public class ProxyController : Controller
    {
        public ActionResult<string> Index()
        {
            var apiUrl = Settings.UrlApi;
           
            HttpRequest original = this.Request;
            original.Query.TryGetValue("Api", out StringValues urlPath);
            HttpWebRequest newRequest = (HttpWebRequest)WebRequest.Create(apiUrl + urlPath);

            newRequest.ContentType = original.ContentType;
            newRequest.Method = original.Method;

            foreach (var headerKey in original.Headers.Keys)
            {
                switch (headerKey)
                {
                    case "Connection":
                    case "Content-Length":
                    case "Date":
                    case "Expect":
                    case "Host":
                    case "If-Modified-Since":
                    case "Range":
                    case "Transfer-Encoding":
                    case "Proxy-Connection":
                        // Let IIS handle these
                        break;

                    case "Accept":
                    case "Content-Type":
                    case "Referer":
                    case "User-Agent":
                    case "AuthKey":
                        // Restricted - copied below
                        break;

                    default:
                        newRequest.Headers[headerKey] = original.Headers[headerKey];
                        break;
                }

            }
            newRequest.Headers["Accept"] = "text/plain";
            newRequest.Headers["Content-Type"] = "text/json";
            if (this.HttpContext.Request.Cookies.TryGetValue("AuthKey", out var authkey))
            {
                newRequest.Headers["AuthKey"] = authkey;
            } 

            if (original.Method != "GET"
                && original.Method != "HEAD"
                && original.ContentLength > 0)
            {
                var syncIOFeature = HttpContext.Features.Get<IHttpBodyControlFeature>();
                syncIOFeature.AllowSynchronousIO = true;
                byte[] originalStream = ReadFully(original.Body);
                syncIOFeature.AllowSynchronousIO = false;
                
                Stream reqStream = newRequest.GetRequestStream();
                reqStream.Write(originalStream, 0, originalStream.Length);
                reqStream.Close();
            }

            try
            {
                HttpWebResponse res = (HttpWebResponse)newRequest.GetResponse();
                using (var reader = new System.IO.StreamReader(res.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    return new OkObjectResult(reader.ReadToEnd());
                }
            }
            catch (WebException e) {

                if (e.Response != null)
                {
                    HttpWebResponse res = (HttpWebResponse)e.Response;
                    using (var reader = new System.IO.StreamReader(res.GetResponseStream(), ASCIIEncoding.ASCII))
                    {
                        switch (res.StatusCode)
                        {
                            case HttpStatusCode.BadRequest:
                                return new BadRequestObjectResult(reader.ReadToEnd());
                            case HttpStatusCode.Unauthorized:
                                return new UnauthorizedObjectResult(reader.ReadToEnd());
                            default:
                                return "Error: " + res.StatusCode + " not mapped in ProxyController.cs";
                        }
                    }
                }
                else 
                {
                    return "Error: " + e.Message;
                }
            }

            return null;
        }

        private static byte[]? ReadFully(Stream input)
        {
            try
            {
                int bytesBuffer = 1024;
                byte[] buffer = new byte[bytesBuffer];
                using (MemoryStream ms = new MemoryStream())
                {
                    int readBytes;
                    while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, readBytes);
                    }
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                // Exception handling here:  Response.Write("Ex.: " + ex.Message);
            }
            return null;
        }
    }
}
