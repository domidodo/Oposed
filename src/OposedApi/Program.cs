using Microsoft.AspNetCore;
using Microsoft.OpenApi.Models;
using OposedApi.Attributes;
using OposedApi.CronJobs;
using OposedApi.Utilities;
using Quartz;
using Quartz.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Oposed API", Version = "v1" });
    c.EnableAnnotations();
    c.OperationFilter<AuthFilter>();
});

StdSchedulerFactory factory = new StdSchedulerFactory();

IScheduler scheduler = await factory.GetScheduler();
await scheduler.Start();

await scheduler.ScheduleJob(
    JobBuilder.Create<Reminder>().Build(),
    TriggerBuilder.Create()
    .WithDailyTimeIntervalSchedule(s =>
         s.WithIntervalInHours(24)
         .OnEveryDay()
         .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(16, 15))
    ).Build()
);

await scheduler.ScheduleJob(
    JobBuilder.Create<Newsletter>().Build(),
    TriggerBuilder.Create()
    .StartNow()
    .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(1, 11, 30))
    .Build()
);


//WebHost.CreateDefaultBuilder(args).UseUrls("http://localhost:5000");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials



app.UseAuthorization();


app.MapControllers();

app.Run();
