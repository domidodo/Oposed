namespace OposedApi
{
    public static class Settings
    {
        #region Database

        public static string DatabasePath { get; } = "Database/Database.db";
        public static string DatabasePath_Event { get; } = "Database/EventDatabase.db";
        public static string DatabasePath_Notification { get; } = "Database/NotificationDatabase.db";

        #endregion // Database

        #region LDAP

        public static string LdapServerHost { get; } = Environment.GetEnvironmentVariable("LDAP_SERVER_HOST");
        public static string LdapServerPort { get; } = Environment.GetEnvironmentVariable("LDAP_SERVER_PORT");
        public static string LdapServerBaseDn { get; } = Environment.GetEnvironmentVariable("LDAP_BASE_DN");
        public static string LdapServerBindDn { get; } = Environment.GetEnvironmentVariable("LDAP_BIND_DN");
        public static string LdapServerBindPassword { get; } = Environment.GetEnvironmentVariable("LDAP_BIND_PASSWORD");
        public static string LdapServerUserFilter { get; } = Environment.GetEnvironmentVariable("LDAP_FILTER_USER");
        public static string LdapServerAdminGroupDn { get; } = Environment.GetEnvironmentVariable("LDAP_ADMIN_GROUP_DN");

        #endregion // LDAP

        #region SMTP
        
        public static string SmtpServerHost { get; } = Environment.GetEnvironmentVariable("SMTP_SERVER_HOST");
        public static string SmtpServerPort { get; } = Environment.GetEnvironmentVariable("SMTP_SERVER_PORT");
        public static string SmtpServerMailAddress { get; } = Environment.GetEnvironmentVariable("SMTP_MAIL_ADDRESS");
        public static string SmtpServerMailPassword { get; } = Environment.GetEnvironmentVariable("SMTP_MAIL_PASSWORD");
        public static string SmtpServerIsSsl { get; } = Environment.GetEnvironmentVariable("SMTP_ENABLE_SSL");
        public static string SmtpServerIgnoreInvalidCertificate { get; internal set; } = Environment.GetEnvironmentVariable("SMTP_IGNORE_INVALID_CERTIFICATE");

        #endregion // SMTP

        public static string BaseUrl { get; } = Environment.GetEnvironmentVariable("OPOSED_BASE_URL");
        public static string PingKey { get; } = Environment.GetEnvironmentVariable("PING_KEY");
        
    }
}
