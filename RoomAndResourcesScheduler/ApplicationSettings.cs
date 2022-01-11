namespace RoomAndResourcesScheduler
{
    public class ApplicationSettings
    {
        private static IConfigurationRoot? _configuration = null;

        public static IConfigurationRoot GetConfiguration() 
        {
            if (_configuration == null)
            {
                _configuration = new ConfigurationBuilder()
                                        .AddJsonFile("appsettings.json")
                                        .Build();
            }
            return _configuration;
        }
    }
}
