namespace Oposed
{
    public static class Settings
    {
        public static string LoginBackground { get; } = Environment.GetEnvironmentVariable("BACKGROUND_LOGIN");
        
        #region URLs
        public static string UrlApi { get; } = Environment.GetEnvironmentVariable("URL_API");
        public static string UrlLogo { get; } = Environment.GetEnvironmentVariable("URL_LOGO");
        public static string UrlImpressum { get; } = Environment.GetEnvironmentVariable("URL_IMPRESSUM");
        
        #endregion // URLs

    }
}
