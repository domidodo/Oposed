using LiteDB;
using OposedApi.Models;

namespace OposedApi.Utilities
{
    public static class NewsletterUtility
    {
        public static void SaveTags(List<Newsletter> newsletter) {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<Newsletter>();
                col.DeleteAll();
                col.InsertBulk(newsletter);
            }
        }

        public static List<Newsletter> GetAllNewsletterSettings()
        {
            List<Newsletter> list = null;
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<Newsletter>();
                list = col.Query().ToList();
            }

            return list;
        }

        public static List<string> GetAllTags()
        {
            List<string> tags = new List<string>();
            List<Newsletter> list = null;
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<Newsletter>();
                list = col.Query().ToList();
            }

            foreach (var newsletter in list) { 
                tags.AddRange(newsletter.Tags);
            }

            return tags;
        }

        public static bool AddNewsletterToUser(User usr, int newsletterId)
        {
            if (usr.DisabledNewsletterIds.Contains(newsletterId))
            {
                usr.DisabledNewsletterIds.Remove(newsletterId);

                using (var db = new LiteDatabase(Settings.DatabasePath))
                {
                    var col = db.GetCollection<User>();
                    return col.Update(usr);
                }
            }

            return true;
        }

        public static bool DeleteNewsletterToUser(User usr, int newsletterId)
        {
            if (!usr.DisabledNewsletterIds.Contains(newsletterId))
            {
                usr.DisabledNewsletterIds.Add(newsletterId);
                using (var db = new LiteDatabase(Settings.DatabasePath))
                {
                    var col = db.GetCollection<User>();
                    return col.Update(usr);
                }
            }
           
            return true;
        }
    }
}
