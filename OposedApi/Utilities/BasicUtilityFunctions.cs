using LiteDB;
using OposedApi.Models;

namespace OposedApi.Utilities
{
    public class BasicUtilityFunctions<T>
    {
        public static int Save(T t)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<T>();
                var id = col.Insert(t);
                return id.AsInt32;
            }
        }

        public static bool Update(T t)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<T>();
                return col.Update(t);
            }
        }

        public static bool DeleteById(int id)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<T>();
                return col.Delete(id);
            }
        }

        public static T GetById(int id)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<T>();
                return col.FindById(id);
            }
        }

        public static List<T> GetAll()
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<T>();
                return col.Query().ToList();
            }
        }
    }
}
