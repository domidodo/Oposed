using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Policy;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OposedPingService
{
    public partial class Ping : ServiceBase
    {
        private EventLog _logger = null;

        public Ping()
        {
            InitializeComponent();

            _logger = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("OposedPing"))
            {
                System.Diagnostics.EventLog.CreateEventSource("OposedPing", "OposedPingLog");
            }
            _logger.Source = "OposedPing";
            _logger.Log = "OposedPingLog";

            
        }

        protected override void OnStart(string[] args)
        {
            var roomId = Settings.Get("roomId", "0", _logger);
            var pingUrlSchema = Settings.Get("pingUrlSchema", "http", _logger, true);
            var pingUrlHost = Settings.Get("pingUrlHost", "local.host", _logger);
            var pingUrlPort = Settings.Get("pingUrlPort", "80", _logger, true);
            var pingKey = Settings.Get("pingKey", "", _logger);

            if (!string.IsNullOrEmpty(roomId) && !string.IsNullOrEmpty(pingUrlSchema) && !string.IsNullOrEmpty(pingUrlHost) && !string.IsNullOrEmpty(pingKey))
            {
                var url = new UriBuilder();
                url.Scheme = pingUrlSchema;
                url.Host = pingUrlHost;
                if (int.TryParse(pingUrlPort, out var port))
                {
                    url.Port = port;
                }
                url.Path = "/Proxy";
                url.Query = $"Api=/Event/Ping/{roomId}";
                
                Task.Factory.StartNew(() =>
                {
                    var doRun = true;
                    while (doRun)
                    {
                        try
                        {
                            HttpWebRequest newRequest = (HttpWebRequest)WebRequest.Create(url.Uri);

                            newRequest.Method = "PUT";
                            newRequest.Headers["X-AuthKey"] = pingKey;

                            newRequest.GetResponse();

                            Thread.Sleep(5*60*1000); // 5 min
                        }
                        catch (Exception e)
                        {
                            _logger.WriteEntry($"Exception: {e.Message} - Url: {url.Uri}", EventLogEntryType.Error);
                            doRun = false;
                        }
                    }

                    Stop();
                });
            }
        }

        protected override void OnStop()
        {

        }
    }
}
