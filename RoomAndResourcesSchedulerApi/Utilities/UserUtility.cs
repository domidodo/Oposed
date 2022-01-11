using LiteDB;
using Microsoft.AspNetCore.Mvc;
using RoomAndResourcesSchedulerApi.Models;

namespace RoomAndResourcesSchedulerApi.Utilities
{
    public static class UserUtility
    {
        public static User GetCurrentUser(HttpContext context)
        {
            return (User)context.Items["User"];
        }

        public static void SaveUser(User usr) {
            using (var db = new LiteDatabase(ApplicationSettings.GetConfiguration().GetValue<string>("DbPath")))
            {
                var col = db.GetCollection<User>();
                col.Update(usr);
            }
        }

        public static List<User> GetAllUsers()
        {
            List<User> list = null;
            using (var db = new LiteDatabase(ApplicationSettings.GetConfiguration().GetValue<string>("DbPath")))
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
            User ldapUser = AuthenticationUtility.GetLdapUser(usr.Mail);

            if (usr != null && ldapUser != null)
            {
                usr.Surname = ldapUser.Surname;
                usr.Name = ldapUser.Name;
                usr.Role = ldapUser.Role;
                usr.Avatar = ldapUser.Avatar;
                usr.Active = ldapUser.Active;
                usr.AuthKey = null;
                usr.LdapDn = ldapUser.LdapDn;

                UserUtility.SaveUser(usr);
            }
            return usr;
        }

        internal static User GetUser(int id)
        {
            using (var db = new LiteDatabase(ApplicationSettings.GetConfiguration().GetValue<string>("DbPath")))
            {
                var col = db.GetCollection<User>();
                return col.Query().Where(x => x.Id == id).FirstOrDefault();
            }
        }
    }
}
