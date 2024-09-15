
namespace NeonArenaMvp
{
    using NeonArenaMvp.Network.Services;
    using NeonArenaMvp.Network.Services.Interfaces;
    using NeonArenaMvp.Network.SignalR;
    using NeonArenaMvp.Persistence;
    using NeonArenaMvp.Persistence.Interfaces;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();

            builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            builder.Services.AddSingleton<ILobbyRepository, InMemoryLobbyRepository>();

            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<ICommService, SignalRCommService>();
            builder.Services.AddSingleton<ILobbyService, LobbyService>();
            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ClientPermissions", policy =>
                {
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                    policy.AllowCredentials();
                    policy.WithOrigins("https://localhost:1337");
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors("ClientPermissions");

            app.UseRouting();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            app.MapHub<GameHub>("/game");

            app.Run();
        }
    }
}