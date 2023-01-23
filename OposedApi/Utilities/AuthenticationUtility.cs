using LiteDB;
using Novell.Directory.Ldap;
using OposedApi.Models;
using System.Text;

namespace OposedApi.Utilities
{
    public static class AuthenticationUtility
    {
        private static Random _random = new Random();


        public static string GetNewAuthId()
        {
            var authId = "";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!()~*-_.";
            bool authIdExists = false;

            using (var db = new LiteDatabase(Settings.DatabasePath))
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
            using (var db = new LiteDatabase(Settings.DatabasePath))
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
            using (var db = new LiteDatabase(Settings.DatabasePath))
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
                        user.Language = auth.Language;
                        col.Insert(user);
                    }
                    else
                    {
                        // User is not in DB and not in LDAP
                        return null;
                    }
                }
                else 
                {
                    user.Language = auth.Language;
                    col.Update(user);
                }
            }

            return user;
        }

        public static User? GetLdapUser(string mail) 
        {
            var serverHost = Settings.LdapServerHost;
            var serverPort = Convert.ToInt32(Settings.LdapServerPort);
            var baseDn = Settings.LdapServerBaseDn;
            var bindDn = Settings.LdapServerBindDn;
            var bindPassword = Settings.LdapServerBindPassword;
            var userFilter = Settings.LdapServerUserFilter;
            var adminGroupDn = Settings.LdapServerAdminGroupDn;

            User? usr = null;

            using (var cn = new LdapConnection())
            {
                cn.Connect(serverHost, serverPort);
                cn.Bind(LdapConnection.LdapV3, bindDn, bindPassword);
               
                string[] attrs = { "memberOf", "sn", "givenName", "jpegPhoto", "nsaccountlock", "krbPasswordExpiration" };
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

                        if (item.GetAttributeSet().ContainsKey("krbPasswordExpiration"))
                        {
                            usr.PasswordExpirationDate = DateTime.ParseExact(item.GetAttribute("krbPasswordExpiration").StringValue, "yyyyMMddHHmmssZ", null);
                        }
                        else
                        {
                            usr.PasswordExpirationDate = DateTime.MaxValue;
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

            var serverIp = Settings.LdapServerHost;
            var serverPort = Convert.ToInt32(Settings.LdapServerPort);
           
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
