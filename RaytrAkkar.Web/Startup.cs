using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Bootstrap.Docker;
using Akka.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RaytrAkkar.Common;
using RaytrAkkar.Web.Data;

namespace RaytrAkkar.Web
{
    public class RaytrakkaListener
    {
        public IActorRef actorRef;

        public RaytrakkaListener(IActorRef actorRef)
        {
            this.actorRef = actorRef;
        }
    }

    public class IdToScene
    {
        public readonly ConcurrentDictionary<string, Bitmap> Collection = new ConcurrentDictionary<string, Bitmap>();
    }

    public class SceneIds
    {
        public readonly ConcurrentBag<string> Collection = new ConcurrentBag<string>();

    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<ActorSystem>( _ => {
                var config = ConfigurationFactory.ParseString(File.ReadAllText("web.hocon"));
                var actorSystem = ActorSystem.Create("raytrakkar", config.BootstrapFromDocker());
                return actorSystem;
            });

            services.AddSingleton<RaytrakkaBridge>( provider => {
                var raytrakkaBridge = new RaytrakkaBridge();
                return raytrakkaBridge;
            });
            services.AddSingleton<RaytrakkaListener>(provider => {
                var raytrakkaBridge = provider.GetService<RaytrakkaBridge>();
                var actorSystem = provider.GetService<ActorSystem>();
                var actorRef = actorSystem.ActorOf(RaytrakkaListenerActor.Props(raytrakkaBridge), "blazor-listener");
                return new RaytrakkaListener(actorRef);
            });

            services.AddSingleton<IdToScene>(_ =>
            {
                return new IdToScene();
            });

            services.AddSingleton<SceneIds>(_ =>
            {
                return new SceneIds();
            });

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
