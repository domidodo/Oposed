using LiteDB;
using Microsoft.AspNetCore.Mvc;
using OposedApi.Models;

namespace OposedApi.Utilities
{
    public static class TemplateUtility
    {
        public static int SaveTemplate(Template templ) {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<Template>();
                var id = col.Insert(templ);
                return id.AsInt32;
            }
        }

        public static List<Template> GetAllTemplates()
        {
            List<Template> list = null;
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<Template>();
                list = col.Query().ToList();
            }

            return list;
        }

        internal static bool DeleteTemplateById(int id)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<Template>();
                return col.Delete(id);
            }
        }
    }
}
