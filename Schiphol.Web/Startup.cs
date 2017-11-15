using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Schiphol.FlightAPI;
using Schiphol.FlightAPI.Services;

namespace Schiphol.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var proxy = new SchipholWebProxy
            {
                 APP_ID = Configuration["FlightAPI:APP_ID"],
                 APP_KEY = Configuration["FlightAPI:APP_KEY"],
                 SchipholApiUrl = Configuration["FlightAPI:ApiUrl"]
            };

            services.AddSingleton<IAirlinesService>(service => new AirlinesService(proxy));
            services.AddSingleton<IFlightsService>(service => new FlightsService(proxy));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            app.UseMvc();
        }
    }
}
