using Serilog;
using TranslateApp.Configurations;
using TranslateApp.Extensions;
using TranslateApp.Logging;
using TranslateApp.Logging.Serilog;

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
try
{

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.AddConfigurations().RegisterSerilog();
    builder.Services.AddControllersWithViews();
    builder.Services
        .AddApplicationServices(builder.Configuration)
        .AddIdentityServices(builder.Configuration);

    var app = builder.Build();

    await app.Services.InitializeDatabasesAsync();

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

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();

}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}
