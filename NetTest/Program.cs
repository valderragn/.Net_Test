using Microsoft.EntityFrameworkCore;
using Serilog;
using NetTest.Data;
using NetTest.Logging;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "NetTest")
    .Enrich.With<CustomEnricher>()
    .WriteTo.File(
        "Logs/myapp.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "<{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}><{SourceContext}><{LineNumber}>[{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
    .CreateLogger();


builder.Host.UseSerilog();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
