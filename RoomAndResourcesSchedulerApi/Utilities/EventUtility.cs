using LiteDB;
using RoomAndResourcesSchedulerApi.Models;

namespace RoomAndResourcesSchedulerApi.Utilities
{
    public static class EventUtility
    {

        internal static List<Event> GetAllEvents()
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                DateTime now = DateTime.Now;

                var timePeriodDb = db.GetCollection<TimePeriod>();
                var timePeriodIds = timePeriodDb.Find(o => now < o.To).GroupBy(o => o.EventId).ToList().Select(o => o.Key).ToList();
            
                var col = db.GetCollection<Event>();
                return FillEventList(col.Find(x => timePeriodIds.Contains(x.Id)).ToList());
            }
        }

        internal static List<Event> GetAllEventsOfResource(int resourceId, bool hitPast = false)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var eventDb = db.GetCollection<Event>();
                if (hitPast)
                {
                    return FillEventList(eventDb.FindAll().ToList());
                }
                else
                {
                    DateTime now = DateTime.Now;
                    var timePeriodDb = db.GetCollection<TimePeriod>();
                    var timePeriodIds = timePeriodDb.Find(o => now < o.To).GroupBy(o => o.EventId).ToList().Select(o => o.Key).ToList();

                    return FillEventList(eventDb.Find(x => x.ResourceId == resourceId && timePeriodIds.Contains(x.Id)).ToList());
                }
            }
        }

        internal static Event? GetNextEventsOfResource(int resourceId)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var eventDb = db.GetCollection<Event>();
               
                DateTime now = DateTime.Now;
                var timePeriodDb = db.GetCollection<TimePeriod>();
                var timePeriodIds = timePeriodDb.Find(o => now < o.To).OrderBy(o => o.From).GroupBy(o => o.EventId).ToList().Select(o => o.Key).ToList();

                if (timePeriodIds.Count > 0)
                {
                    return FillEvent(eventDb.FindById(timePeriodIds[0]), db);
                }
            }
            return null;
        }

        internal static Event GetEventById(int id)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var col = db.GetCollection<Event>();
                return FillEvent(col.FindById(id), db);
            }
        }

        internal static Event? AddEvent(Event evt)
        {
            if (EventUtility.GetBlockedTimePeriods(evt.ResourceId, evt.Schedule).Count > 0)
            {
                return null;
            }

            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var timePeriodDb = db.GetCollection<TimePeriod>();
                var eventDb = db.GetCollection<Event>();

                var newEventId = eventDb.Insert(evt);
                evt.Id = newEventId.AsInt32;

                foreach (var time in evt.Schedule) 
                {
                    time.EventId = evt.Id;
                    var newTimeId = timePeriodDb.Insert(time);
                    evt.TimePeriodIds.Add(newTimeId.AsInt32);
                }

                eventDb.Update(evt);

                return FillEvent(evt, db);
            }
        }

        internal static bool UpdateEvent(Event evt)
        {
            if (evt.Schedule != null) 
            {
                var ownScheduleIds = evt.Schedule.Select(x => x.Id).ToList();
                var blockedSchedules = EventUtility.GetBlockedTimePeriods(evt.ResourceId, evt.Schedule);
                blockedSchedules = blockedSchedules.Where(x => !ownScheduleIds.Contains(x.Id)).ToList();

                if (blockedSchedules.Count > 0) {
                    return false;
                }
            }

            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var timePeriodDb = db.GetCollection<TimePeriod>();
                var eventDb = db.GetCollection<Event>();

                foreach (var time in evt.Schedule)
                {
                    if (time.Id <= 0)
                    {
                        time.EventId = evt.Id;
                        var newTimeId = timePeriodDb.Insert(time);
                        evt.TimePeriodIds.Add(newTimeId.AsInt32);
                    }
                    else if (evt.TimePeriodIds.Contains(time.Id))
                    {
                        timePeriodDb.Update(time);
                    }
                    else 
                    {
                        timePeriodDb.Delete(time.Id);
                    }
                }

                return eventDb.Update(evt);
            }
        }

        internal static bool DeleteEventById(int id)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var timePeriodDb = db.GetCollection<TimePeriod>();
                var eventDb = db.GetCollection<Event>();

                timePeriodDb.DeleteMany(e => e.EventId == id);

                return eventDb.Delete(id);
            }
        }

        internal static List<TimePeriod> GetBlockedTimePeriods(int resourceId, List<TimePeriod> time)
        {
            List<TimePeriod> ret = new List<TimePeriod>();
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var timePeriodDb = db.GetCollection<TimePeriod>();
                var eventDb = db.GetCollection<Event>();
                foreach (var t in time) {
                    var eventsIdInSameTime = timePeriodDb.Find(o => o.From < t.To && o.To > t.From).Select(x => x.EventId).ToList();

                    var exists = eventDb.Query().Where(o => o.ResourceId == resourceId && eventsIdInSameTime.Contains(o.Id)).Exists();

                    if (exists)
                    {
                        ret.Add(t);
                    }
                }
            }
            return ret;
        }


        private static List<Event> FillEventList(List<Event> events)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                var userDb = db.GetCollection<User>();
                var resourceDb = db.GetCollection<Resource>();
                var scheduleDb = db.GetCollection<TimePeriod>();

                foreach (var eventItem in events)
                {
                    FillEvent(eventItem, db);
                }

                return events;
            }
        }

        private static Event FillEvent(Event eventItem, LiteDatabase conn)
        {
            var userDb = conn.GetCollection<User>();
            var resourceDb = conn.GetCollection<Resource>();
            var scheduleDb = conn.GetCollection<TimePeriod>();

            if (eventItem.Resource == null)
            {
                eventItem.Resource = resourceDb.FindById(eventItem.ResourceId);
            }

            if (eventItem.Organizer == null)
            {
                eventItem.Organizer = userDb.FindById(eventItem.OrganizerId);
                eventItem.Organizer.AuthKey = "";
                eventItem.Organizer.LdapDn = "";
            }

            if (eventItem.Visitors == null)
            {
                eventItem.Visitors = userDb.Find(x => eventItem.VisitorIds.Contains(x.Id)).ToList();
                foreach (var visiror in eventItem.Visitors) 
                {
                    visiror.AuthKey = "";
                    visiror.LdapDn = "";
                }
            }

            if (eventItem.Schedule == null)
            {
                eventItem.Schedule = scheduleDb.Find(x => eventItem.TimePeriodIds.Contains(x.Id)).ToList();
            }
            
            return eventItem;
        }
    }
}
