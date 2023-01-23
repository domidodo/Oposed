using LiteDB;
using OposedApi.Models;

namespace OposedApi.Utilities
{
    public static class TemplateUtility
    {
        public static Template GetTemplateById(int id)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<Template>();
                return col.FindById(id);
            }
        }

        public static List<Template> GetTemplatesByUserId(int userId)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<Template>();

                return col.Find(x => x.UserId == userId || x.UserId == 0).ToList();
            }
        }

        public static bool SaveTemplate(Template templ) {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<Template>();

                var oldTemplate = col.FindOne(x => x.Data.Name == templ.Data.Name && x.UserId == templ.UserId);
                if (oldTemplate != null)
                {
                    templ.Id = oldTemplate.Id;
                    return col.Update(templ);
                }
                
                var id = col.Insert(templ);
                return id.AsInt32 > 0;
            }
        }
    }
}
