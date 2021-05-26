using LudoDataAccess;
using LudoDataAccess.Database;
using LudoGame.GameEngine.Classes;
using LudoGame.GameEngine.Configuration;
using LudoGame.GameEngine.Interfaces;
using LudoTranslation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LudoWeb.ChatModels;
using LudoWeb.GameClasses;
using LudoWeb.GameInterfaces;
using Microsoft.AspNetCore.SignalR;

namespace LudoWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddSignalR();
            services.AddSingleton<LudoContext>();
            services.AddSingleton<ILudoRepository, DbRepository>();
            services.AddSingleton<ChatHubData>();
            services.AddSingleton<IBoardOrm, BoardOrm>();
            services.AddSingleton<IDatabaseManagement, DatabaseManagement>();
            services.AddSingleton<AbstractFactory, LudoFactory>();
            services.AddSingleton<LudoNetworkFactory>();
            services.AddSingleton<GameNetworkManager>();
            services.AddSingleton<IHtmlBoardBuilder, HtmlBoardBuilder>();
            services.AddSingleton<TranslationEngine>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<ChatHub>("/chatHub");
                endpoints.MapHub<GameHub>("/gameHub");
                endpoints.MapHub<AccountHub>("/accountHub");
            });
        }
    }
}
