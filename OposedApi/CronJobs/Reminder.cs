using OposedApi.Models;
using OposedApi.Utilities;
using Quartz;

namespace OposedApi.CronJobs
{
    public class Reminder : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() => {
                DateTime dt = DateTime.Today.AddDays(1).AddHours(23).AddMinutes(59);
                var eventlist = EventUtility.GetAllEvents(dt);

                var userList = UserUtility.GetAllUsers();
                foreach (var usr in userList)
                {
                    if (!usr.Active)
                        break;

                    List<Event> userEvents = new List<Event>();
                    foreach (var evt in eventlist)
                    {
                        if (evt.Room != null && evt.VisitorIds.Contains(usr.Id))
                        {
                            userEvents.Add(evt);
                        }
                    }

                    if (userEvents.Count > 0)
                    {
                        MailSenderUtility.Send(usr, new MailType.Reminder(userEvents));
                    }
                }
            });
        }
    }
}
