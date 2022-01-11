using LiteDB;
using RoomAndResourcesSchedulerApi.Models;

namespace RoomAndResourcesSchedulerApi.Utilities
{
    public static class EventUtility
    {

        internal static List<Event> GetAllEvents()
        {
            using (var db = new LiteDatabase(ApplicationSettings.GetConfiguration().GetValue<string>("DbPath")))
            {
                DateTime now = DateTime.Now;
                var col = db.GetCollection<Event>();
                return col.Query()
                            .Where(x => x.Schedule.Select(o => o.To > now || o.OpenEnd).Contains(true))
                            .ToList();
            }
        }

        internal static List<Event> GetAllEventsOfResource(int resourceId, bool hitPast = false)
        {
            using (var db = new LiteDatabase(ApplicationSettings.GetConfiguration().GetValue<string>("DbPath")))
            {
                DateTime now = DateTime.Now;
                var col = db.GetCollection<Event>();
                var list = new List<Event>();
                if (hitPast)
                {
                    list = col.Query()
                                .Where(x => x.ResourceId == resourceId)
                                .ToList();
                }
                else
                {
                    list = col.Query()
                                .Where(x => x.ResourceId == resourceId)
                                .Where(x => x.Schedule.Select(o => o.To > now || o.OpenEnd).Contains(true))
                                .ToList();
                }

                list.OrderBy(x => x.Ticks);

                return list;
            }
        }

        internal static Event GetEventById(int id)
        {
            using (var db = new LiteDatabase(ApplicationSettings.GetConfiguration().GetValue<string>("DbPath")))
            {
                var col = db.GetCollection<Event>();
                return col.Query().Where(x => x.Id == id).FirstOrDefault();
            }
        }

        internal static Event? AddEvent(Event evt)
        {
            if (EventUtility.GetBlockedTimePeriods(evt.ResourceId, evt.Schedule).Count > 0)
            {
                return null;
            }

            using (var db = new LiteDatabase(ApplicationSettings.GetConfiguration().GetValue<string>("DbPath")))
            {
                var col = db.GetCollection<Event>();
                var newId = col.Insert(evt);
                evt.Id = newId.AsInt32;
                return evt;
            }
        }

        internal static bool UpdateEvent(Event evt)
        {
            if (EventUtility.GetBlockedTimePeriods(evt.ResourceId, evt.Schedule).Count > 0) 
            {
                return false;
            }

            using (var db = new LiteDatabase(ApplicationSettings.GetConfiguration().GetValue<string>("DbPath")))
            {
                var col = db.GetCollection<Event>();
                return col.Update(evt);
            }
        }

        internal static bool DeleteEventById(int id)
        {
            using (var db = new LiteDatabase(ApplicationSettings.GetConfiguration().GetValue<string>("DbPath")))
            {
                var col = db.GetCollection<Event>();
                return col.Delete(id);
            }
        }

        internal static List<TimePeriod> GetBlockedTimePeriods(int resourceId, List<TimePeriod> time)
        {
            List<TimePeriod> ret = new List<TimePeriod>();
            using (var db = new LiteDatabase(ApplicationSettings.GetConfiguration().GetValue<string>("DbPath")))
            {
                var col = db.GetCollection<Event>();
                foreach (var t in time) {
                    bool exists = col.Query()
                                    .Where(x => x.ResourceId == resourceId)
                                    .Where(x => x.Schedule.Select(o => o.From < t.To && (o.To > t.From || o.OpenEnd)).Contains(true))
                                    .Exists();
                    if (exists)
                    {
                        ret.Add(t);
                    }
                }
            }
            return ret;
        }
    }
}
