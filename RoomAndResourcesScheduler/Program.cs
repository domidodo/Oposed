using Microsoft.AspNetCore.Localization;
using RoomAndResourcesScheduler.Localization;
using System.Globalization;
using XLocalizer;
using XLocalizer.Routing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages()
        .AddXLocalizer<LocSource>(ops =>
        {
            ops.ResourcesPath = "Localization";
            ops.TranslateFromCulture = "en";
        });
builder.Services.Configure<RequestLocalizationOptions>(ops =>
{
    var cultures = new CultureInfo[] {
       new CultureInfo("de"),
       new CultureInfo("en")
    };
    ops.SupportedCultures = cultures;
    ops.SupportedUICultures = cultures;
    ops.DefaultRequestCulture = new RequestCulture("en");

    // Optional: add custom provider to support localization 
    // based on route value
    ops.RequestCultureProviders.Insert(0, new RouteSegmentRequestCultureProvider(cultures));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseRequestLocalization();

app.Run();