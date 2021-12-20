using FluentValidation.AspNetCore;
using Hangfire.Dashboard;
using Hangfire.UI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Hangfire.UI
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
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseRedisStorage(Configuration.GetConnectionString("Redis")));
            
            var provider = new PhysicalFileProvider(Configuration.GetSection("Store")["RootPath"]); 
            services.AddSingleton<IFileProvider>(provider);
            services.AddScoped<IArchiveService, ArchiveService>();
            services.AddControllersWithViews()
                .AddFluentValidation(s =>
                {
                    s.ImplicitlyValidateChildProperties = true;
                    s.RegisterValidatorsFromAssemblyContaining(typeof(Startup));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new [] {  new AllowAllConnectionsFilter () },
                IgnoreAntiforgeryToken = true
            });
            
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(e =>
            {
                e.MapControllers();
            });
        }
    }
    public class AllowAllConnectionsFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            // Allow outside. You need an authentication scenario for this part.
            // DON'T GO PRODUCTION WITH THIS LINES.
            return true;
        }
    }
}
