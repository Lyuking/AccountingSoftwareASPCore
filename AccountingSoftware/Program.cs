using AccountingSoftware.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using AccountingSoftware.Data;
using AccountingSoftware.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Quartz;
using AccountingSoftware.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ASWDB")));
builder.Services.AddDbContext<AccountingSoftwareIdentityContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ASWDB")));
builder.Services.AddIdentity<AccountingSoftwareUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false).
AddEntityFrameworkStores<AccountingSoftwareIdentityContext>();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.AccessDeniedPath = new PathString("/Identity/Account/AccessDenied");
    opt.LoginPath = new PathString("/Identity/Account/Login");
});
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    var jobKey = new JobKey("OutDateReportSender");

    q.AddJob<OutDateReportSender>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(t => t
    .ForJob(jobKey)
    .WithIdentity("OutDateReportSender-trigger")
    .StartNow()
    .WithSimpleSchedule(x => x.WithIntervalInHours(24)// настраиваем выполнениедействия через 1 минуту
    .RepeatForever()) // бесконечное повторение
    );
}
);
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    var jobKey = new JobKey("ExpiredRepostSender");

    q.AddJob<ExpiredReportSender>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(t => t
    .ForJob(jobKey)
    .WithIdentity("ExpiredRepostSender-trigger")
    .StartNow()
    .WithSimpleSchedule(x => x.WithIntervalInHours(24)// настраиваем выполнениедействия через 1 минуту
    .RepeatForever()) // бесконечное повторение
    );
}
);

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
