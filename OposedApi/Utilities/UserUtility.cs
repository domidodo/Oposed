using LiteDB;
using Microsoft.AspNetCore.Mvc;
using OposedApi.Models;

namespace OposedApi.Utilities
{
    public static class UserUtility
    {
        public static User GetCurrentUser(HttpContext context)
        {
            return (User)context.Items["User"];
            if (context.Items.TryGetValue("User", out object? usr) && usr != null)
            {
                return (User)usr;
            }
            throw new Exception("User not found");
        }

        public static void SaveUser(User usr) {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<User>();
                col.Update(usr);
            }
        }

        public static List<User> GetAllUsers()
        {
            List<User> list = null;
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<User>();
                list = col.Query().ToList();
            }

            foreach (var item in list)
            {
                item.AuthKey = null;
                item.LdapDn = null;
            }

            return list;
        }

        public static User Refetch(User usr)
        {
            User? ldapUser = AuthenticationUtility.GetLdapUser(usr.Mail);

            if (usr != null && ldapUser != null)
            {
                usr.Surname = ldapUser.Surname;
                usr.Name = ldapUser.Name;
                usr.Role = ldapUser.Role;
                usr.Avatar = ldapUser.Avatar;
                usr.Active = ldapUser.Active;
                usr.PasswordExpirationDate = ldapUser.PasswordExpirationDate;
                usr.AuthKey = null;
                usr.LdapDn = ldapUser.LdapDn;

                UserUtility.SaveUser(usr);
            }
            return usr;
        }

        internal static User GetUser(int id)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<User>();
                return col.Query().Where(x => x.Id == id).FirstOrDefault();
            }
        }

        internal static List<User> GetUsers(List<int> ids)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<User>();
                return col.Query().Where(x => ids.Contains(x.Id)).ToList();
            }
        }
    }
}
