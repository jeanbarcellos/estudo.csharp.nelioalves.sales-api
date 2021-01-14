using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SalesWebApi.Data;
using SalesWebApi.Services;

namespace SalesWebApi
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
            services.AddDbContext<SalesWebApiContext>(
              options => options.UseNpgsql(Configuration.GetConnectionString("Default"))
            );
            services.AddScoped<SalesWebApiContext>();

            services.AddScoped<SeedingService>();
            services.AddScoped<DepartmentService>();
            services.AddScoped<SellerService>();
            services.AddScoped<SalesRecordService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SalesWebApi", Version = "v1" });
            });
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            SeedingService seedingService
        )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SalesWebApi v1"));

                // Popula o banco de dados default
                seedingService.Seed();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
