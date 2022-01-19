using LiteDB;
using RoomAndResourcesSchedulerApi.Models;

namespace RoomAndResourcesSchedulerApi.Utilities
{
    public class ResourceUtility
    {
        public static int SaveResource(Resource resource)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<Resource>();
                var id = col.Insert(resource);
                return id.AsInt32;
            }
        }

        public static bool UpdateResource(Resource resource)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<Resource>();
                return col.Update(resource);
            }
        }

        public static bool DeleteResourceById(int id)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<Resource>();
                return col.Delete(id);
            }
        }

        public static Resource GetResourceById(int id)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<Resource>();
                return col.Query().Where(x => x.Id == id).FirstOrDefault();
            }
        }

        public static List<Resource> GetAllResources()
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<Resource>();
                return col.Query().ToList();
            }
        }
    }
}
