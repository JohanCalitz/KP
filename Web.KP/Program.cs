using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using Web.Services;
using Web.Services.Helpers;
using Web.Services.Interfaces;

Log.Logger = new LoggerConfiguration()
                  .Enrich.FromLogContext()
                  .WriteTo.Console()
                  .CreateLogger();

Log.Information("Starting web api");
try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    Log.Information("Adding controllers");
    builder.Services.AddControllersWithViews();

    Log.Information("Adding httpclients");
    builder.Services.AddHttpClient("ApiClient", client =>
    {
        client.BaseAddress = new Uri("https://localhost:7146/");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    }).AddHttpMessageHandler<TokenHandler>();

    Log.Information("Adding services");
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IGroupService, GroupService>();
    builder.Services.AddScoped<IPermissionService, PermissionService>();
    builder.Services.AddScoped<IAccountSerivce, AccountSerivce>();

    builder.Services.AddTransient<TokenHandler>();
    builder.Services.AddSession(options =>
    {
        options.Cookie.IsEssential = true;
        options.IdleTimeout = TimeSpan.FromMinutes(30);
    });
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index"; 
        options.AccessDeniedPath = "/Home/Error";
    });
    builder.Services.AddHttpContextAccessor();

    WebApplication app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }



    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();
    app.UseAuthentication();
    app.UseSession();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}/{id?}");

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup error");
    Log.CloseAndFlush();
    Console.ReadLine();
}
