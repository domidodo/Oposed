using LiteDB;
using OposedApi.Models;

namespace OposedApi.Utilities
{
    public static class EventUtility
    {

        internal static List<Event> GetAllEvents(DateTime? until = null)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                DateTime now = DateTime.Now;

                var timePeriodDb = db.GetCollection<TimePeriod>();
                var timePeriodIds = timePeriodDb.Find(o => now <= o.To && (until == null || until >= o.From)).GroupBy(o => o.EventId).ToList().Select(o => o.Key).ToList();
            
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

                    return FillEventList(eventDb.Find(x => (x.RoomId == resourceId || x.DevicesIds.Contains(resourceId)) && timePeriodIds.Contains(x.Id)).ToList());
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

                foreach (var id in timePeriodIds) {
                    var evet = eventDb.FindById(id);
                    if (evet != null && (evet.RoomId == resourceId || evet.DevicesIds.Contains(resourceId))) { 
                        return FillEvent(evet, db);
                    }
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
            if (evt.Schedule != null)
            {
                foreach (var timePeriod in evt.Schedule)
                {
                    if (EventUtility.IsResourceBlockedByEvent(evt.RoomId, timePeriod))
                    {
                        return null;
                    }

                    foreach (var devicesId in evt.DevicesIds)
                    {
                        if (EventUtility.IsResourceBlockedByEvent(devicesId, timePeriod))
                        {
                            return null;
                        }
                    }
                }
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
                foreach (var timePeriod in evt.Schedule)
                {
                    if(EventUtility.IsResourceBlockedByEvent(evt.RoomId, timePeriod, new List<int>(){ evt.Id })){
                        return false;
                    }

                    foreach (var devicesId in evt.DevicesIds)
                    {
                        if (EventUtility.IsResourceBlockedByEvent(devicesId, timePeriod, new List<int>() { evt.Id }))
                        {
                            return false;
                        }
                    }
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
        
        internal static bool IsResourceBlockedByEvent(int resourceId, TimePeriod time, List<int>? excludedEventIds = null)
        {
            if (excludedEventIds == null)
            {
                excludedEventIds = new List<int>();
            }

            using var db = new LiteDatabase(Settings.DatabasePath);
            var timePeriodDb = db.GetCollection<TimePeriod>();
            var eventDb = db.GetCollection<Event>();
                
            var eventsIdInSameTime = timePeriodDb.Find(o => o.From < time.To && o.To > time.From && o.EventId.HasValue && !excludedEventIds.Contains(o.EventId.Value)).Select(x => x.EventId).ToList();
                
            return eventDb.Find(o => (o.RoomId == resourceId || o.DevicesIds.Contains(resourceId)) && eventsIdInSameTime.Contains(o.Id)).Any();
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

            if (eventItem.Room == null)
            {
                eventItem.Room = resourceDb.FindById(eventItem.RoomId);
            }

            if (eventItem.Devices == null)
            {
                eventItem.Devices = resourceDb.Find(x => eventItem.DevicesIds.Contains(x.Id)).ToList();
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

        public static void PingByRoomId(int roomId)
        {
            using (var db = new LiteDatabase(Settings.DatabasePath))
            {
                DateTime now = DateTime.Now;

                var timePeriodDb = db.GetCollection<TimePeriod>();
                var eventIds = timePeriodDb.Find(o => now <= o.To && now >= o.From).GroupBy(o => o.EventId).ToList().Select(o => o.Key).ToList();

                var events = db.GetCollection<Event>();
                var currentRunningEvent = events.Find(o => o.RoomId == roomId && eventIds.Contains(o.Id)).FirstOrDefault();

                if (currentRunningEvent != null)
                {
                    var timePeriod = timePeriodDb.Find(o => o.EventId == currentRunningEvent.Id && now <= o.To && now >= o.From).FirstOrDefault();
                    if (timePeriod != null)
                    {
                        timePeriod.LastPing = now;
                        timePeriodDb.Update(timePeriod);
                    }
                }
            }
        }
    }
}
