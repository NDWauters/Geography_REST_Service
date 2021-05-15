using BusinessLogicLayer.BusinessLogicServices;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            services.AddScoped<RiverRepo>();
            services.AddScoped<ContinentRepo>();
            services.AddScoped<CountryRepo>();
            services.AddScoped<CityRepo>();

            services.AddSingleton<GeographyContext>();
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
