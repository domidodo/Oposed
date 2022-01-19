namespace RoomAndResourcesScheduler
{
    public static class Settings
    {
        #region URLs
        public static string UrlApi { get; } = Environment.GetEnvironmentVariable("URL_API");
        public static string UrlLogo { get; } = Environment.GetEnvironmentVariable("URL_LOGO");
        public static string UrlImpressum { get; } = Environment.GetEnvironmentVariable("URL_IMPRESSUM");
        
        #endregion // URLs

    }
}
