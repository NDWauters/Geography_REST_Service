using BusinessLogicLayer.BusinessLogicServices;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Geography_REST_Service.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Geography_REST_Service
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

            services.AddControllers().AddNewtonsoftJson();

            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IContinentService, ContinentService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IRiverService, RiverService>();

            services.AddScoped<IRiverRepo,RiverRepo>();
            services.AddScoped<IContinentRepo,ContinentRepo>();
            services.AddScoped<ICountryRepo,CountryRepo>();
            services.AddScoped<ICityRepo,CityRepo>();

            services.AddSingleton<GeographyContext>();
            services.AddSingleton<ILogger>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
