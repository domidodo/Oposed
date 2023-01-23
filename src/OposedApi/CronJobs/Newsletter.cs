using OposedApi.Models;
using OposedApi.Utilities;
using Quartz;

namespace OposedApi.CronJobs
{
    public class Newsletter : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() => {
                DateTime dt = DateTime.Today.AddMonths(1).AddDays(14).AddHours(23).AddMinutes(59);
                var eventlist = EventUtility.GetAllEvents(dt);

                if (eventlist.Count <= 0)
                    return;
                
                var userList = UserUtility.GetAllUsers();
                foreach (var usr in userList)
                {
                    if (!usr.Active)
                        break;

                    List<Event> userEvents = new List<Event>();
                    foreach (var evt in eventlist)
                    {
                        if (evt.IsPrivate)
                            break;

                        List<string> allowedTags = NewsletterUtility.GetAllSubscribedTagsByUser(usr);
                        if (evt.Room != null && evt.Tags.FindIndex(x => allowedTags.Contains(x)) >= 0)
                        {
                            userEvents.Add(evt);
                        }
                    }

                    if (userEvents.Count > 0)
                    {
                        MailSenderUtility.Send(usr, new MailType.Newsletter(userEvents));
                    }
                }
            });
        }
    }
}
