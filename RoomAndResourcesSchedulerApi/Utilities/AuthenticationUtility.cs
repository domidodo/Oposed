using LiteDB;
using Novell.Directory.Ldap;
using RoomAndResourcesSchedulerApi.Models;


namespace RoomAndResourcesSchedulerApi.Utilities
{
    public static class AuthenticationUtility
    {
        private static Random _random = new Random();


        public static string GetNewAuthId()
        {
            var authId = "";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!()~*-_.";
            bool authIdExists = false;

            using (var db = new LiteDatabase(ApplicationSettings.GetConfiguration().GetValue<string>("DbPath")))
            {
                do
                {
                    authId = new string(Enumerable.Repeat(chars, 50).Select(s => s[_random.Next(s.Length)]).ToArray());

                    var col = db.GetCollection<User>();

                    authIdExists = col.Query()
                        .Where(x => x.Active)
                        .Where(x => x.AuthKey.Equals(authId))
                        .Exists();

                } while (authIdExists);
            }

            return authId;
        }

        public static User GetUserByAuthId(string authId)
        {
            using (var db = new LiteDatabase(ApplicationSettings.GetConfiguration().GetValue<string>("DbPath")))
            {
                var col = db.GetCollection<User>();
                
                return col.Query()
                    .Where(x => x.Active)
                    .Where(x => x.AuthKey.Equals(authId))
                    .FirstOrDefault();
            }
        }

        public static User? GetUserByAuthentication(Authentication auth)
        {
            User? user = null;
            using (var db = new LiteDatabase(ApplicationSettings.GetConfiguration().GetValue<string>("DbPath")))
            {
                var col = db.GetCollection<User>();

                user = col.Query()
                            .Where(x => x.Mail.Equals(auth.Mail))
                            .FirstOrDefault();

                if (user == null) 
                {
                    // User is not in DB. Search in LDAP
                    user = GetLdapUser(auth.Mail);
                    if (user != null)
                    {
                        // User found in LDAP and import to DB
                        col.Insert(user);
                    }
                    else 
                    {
                        // User is not in DB and not in LDAP
                        return null;
                    }
                }
            }

            return user;
        }

        public static User? GetLdapUser(string mail) 
        {
            var ldapConf = ApplicationSettings.GetConfiguration().GetSection("Ldap");

            var serverIp = ldapConf.GetValue<string>("ServerIp");
            var serverPort = ldapConf.GetValue<int>("ServerPort");
            var baseDn = ldapConf.GetValue<string>("BaseDn");
            var bindDn = ldapConf.GetValue<string>("BindDn");
            var bindPassword = ldapConf.GetValue<string>("BindPassword");
            var userFilter = ldapConf.GetValue<string>("UserFilter");
            var adminGroupDn = ldapConf.GetValue<string>("AdminGroupDn");

            User? usr = null;

            using (var cn = new LdapConnection())
            {
                cn.Connect(serverIp, serverPort);
                cn.Bind(LdapConnection.LdapV3, bindDn, bindPassword);
               
                string[] attrs = { "memberOf", "sn", "givenName", "jpegPhoto", "nsaccountlock" };
                string filter = "(&(" + userFilter + ")(mail=" + mail + "))";
                var res = cn.Search(baseDn, LdapConnection.ScopeSub, filter, attrs, false);

                if (res.HasMore())
                {
                    try
                    {
                        LdapEntry item = res.Next();

                        usr = new User();
                        usr.LdapDn = item.Dn;
                        if (item.GetAttributeSet().ContainsKey("nsaccountlock"))
                        {
                            usr.Active = !Convert.ToBoolean(item.GetAttribute("nsaccountlock").StringValue);
                        }
                        else 
                        {
                            usr.Active = true;
                        }
                        if (item.GetAttributeSet().ContainsKey("jpegPhoto"))
                        {
                            usr.Avatar = "data:image/png;base64," + Convert.ToBase64String(item.GetAttribute("jpegPhoto").ByteValue);
                        }
                        usr.Surname = item.GetAttribute("sn").StringValue;
                        usr.Name = item.GetAttribute("givenName").StringValue;
                        usr.Mail = mail;

                        if (item.GetAttribute("memberOf").StringValueArray.Contains(adminGroupDn))
                        {
                            usr.Role = Enum.UserRole.Admin;
                        }
                        else 
                        {
                            usr.Role = Enum.UserRole.User;
                        }
                    }
                    catch (LdapException e)
                    {
                        throw e;
                    }
                }
            }

            return usr;
        }

        public static bool CheckPassword(User usr, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            var ldapConf = ApplicationSettings.GetConfiguration().GetSection("Ldap");

            var serverIp = ldapConf.GetValue<string>("ServerIp");
            var serverPort = ldapConf.GetValue<int>("ServerPort");

            bool correctPassword = false;
            using (var cn = new LdapConnection())
            {
                cn.Connect(serverIp, serverPort);
                try
                {
                    cn.Bind(LdapConnection.LdapV3, usr.LdapDn, password);
                    correctPassword = true;
                }
                catch (LdapException)
                { }
            }

            return correctPassword;
        }
    }
}
