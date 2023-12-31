using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TopLearn.DataLayer.Context;
using TopLearn.IocLayer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddControllersWithViews(options => options.EnableEndpointRouting = false);



#region DataBase Context

builder.Services.AddDbContext<TopLearnContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TopLearnConnection"));
});

#endregion


#region IOC Service

RegisterServices(builder.Services);


static void RegisterServices(IServiceCollection services)
{
    DependencyContainers.RegisterServicse(services);
}


#endregion


#region Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

}).AddCookie(option =>
    {
        option.LoginPath = "/Login";
        option.LogoutPath = "/Logout";
        option.ExpireTimeSpan = TimeSpan.FromDays(4);
    });

#endregion



var app = builder.Build();

app.UseStaticFiles();
app.UseAuthentication();


app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "areas",
        template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"

    );
    routes.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
});

//app.MapControllerRoute(
//     name: "areas",
//     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//     );

//app.UseMvcWithDefaultRoute();

app.MapGet("/", () => "Hello World!");

app.Run();
