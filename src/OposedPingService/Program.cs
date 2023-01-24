using System.ServiceProcess;


namespace OposedPingService
{
    // https://learn.microsoft.com/de-de/dotnet/framework/windows-services/walkthrough-creating-a-windows-service-application-in-the-component-designer

    internal static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main()
        {
            Settings.Init();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Ping()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
