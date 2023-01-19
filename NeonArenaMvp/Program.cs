using NeonArenaMvp.Network.Services.Implementations;
using NeonArenaMvp.Network.Services.Interfaces;
using NeonArenaMvp.Network.SignalR;

namespace NeonArenaMvp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<ICommunicationService, CommunicationService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<ILobbyService, LobbyService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapHub<GameHub>("/gameHub");

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}