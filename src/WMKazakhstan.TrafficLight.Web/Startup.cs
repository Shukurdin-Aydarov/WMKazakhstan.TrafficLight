using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

using WMKazakhstan.TrafficLight.Core.Interfaces;
using WMKazakhstan.TrafficLight.Core.Services;
using WMKazakhstan.TrafficLight.Infrastructure;
using WMKazakhstan.TrafficLight.Infrastructure.DataStorage;
using WMKazakhstan.TrafficLight.Web.Middlewares;
using WMKazakhstan.TrafficLight.Web.Models;

namespace WMKazakhstan.TrafficLight.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

                    JsonConvert.DefaultSettings = () => options.SerializerSettings;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = ctx =>
                    {
                        var error = ctx.ModelState.Values.First().Errors.First();

                        return new OkObjectResult(new Error(error.ErrorMessage));
                    };
                });

            services.AddDbContext<Context>(
                options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IStorage, EfStorage>();
            services.AddScoped<IBrokenSectionsExpert, BrokenSectionsExpert>();
            services.AddScoped<IDigitPredictor, DigitPredictor>();
            services.AddScoped<ITrafficLightTimePredictor, TrafficLightTimePredictor>();
            services.AddScoped<ITrafficLightService, TrafficLightService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
