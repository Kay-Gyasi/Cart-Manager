using FluentValidation.AspNetCore;
using Hubtel.ECommerce.API.Core.Application;
using Hubtel.ECommerce.API.Core.Application.Carts;
using Hubtel.ECommerce.API.Core.Application.Items;
using Hubtel.ECommerce.API.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace Hubtel.ECommerce.API
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
            services.AddControllers(opt =>
            {
                opt.Filters.Add<ValidationAttribute>();
            });
            services.AddHttpContextAccessor();
            services.AddSingleton<AuditEntitiesInterceptor>();
            services.RegisterDbContext(Configuration)
                .AddProcessors()
                .AddSwagger()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.RunMigrations();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hubtel - Cart Manager v1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

    public static class Installers
    {

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hubtel - Cart Manager", Version = "v1" });
            });

            return services;
        }

        public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var saveChangesInterceptor = sp.GetService<AuditEntitiesInterceptor>();
                options.UseNpgsql(configuration.GetConnectionString("Default"), opts =>
                {
                    opts.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                }).AddInterceptors(saveChangesInterceptor);
            });
            return services;
        }

        public static IServiceCollection AddProcessors(this IServiceCollection services)
        {
            services.AddScoped<CartProcessor>()
                .AddScoped<ItemProcessor>()
                .AddScoped<Initializer>();
            return services;
        }

        public static void RunMigrations(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
            if (Environment.CommandLine.Contains("migrations add")) return;
            try
            {
                context?.Database.Migrate();
                var initializer = serviceScope.ServiceProvider.GetService<Initializer>();
                initializer?.Initialize().GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
