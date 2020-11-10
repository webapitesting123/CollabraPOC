using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FabrikamResidences_Activities.Data
{
    public static class DataInitializer
    {
        internal static void RegisterDataService(IServiceCollection services, IConfiguration configuration)
        {
            //This is a Singleton due to in memory data
            // services.AddSingleton<IPortalRepository, PortalRepository_Memory>();     

            // Uncomment after completing Module 3 Demo 1 
            //  and ready to use the ADONet verion of the repository
            // services.AddScoped<IPortalRepository, PortalRepository_ADONet>();

            // Uncomment after completing Module 3 Demo 2
            //  and ready to use the EF Core version of the repository
            services.AddScoped<IPortalRepository, PortalRepository_EFCore>();
            services.AddDbContext<PortalContext>(options => options.UseSqlServer(configuration.GetConnectionString("AzureDB")));
            
           }

        public static IWebHost InitializeDatabase(this IWebHost webHost)
        {
            //Basis for Application Startup Configuration being moved to the Program.cs
            // https://github.com/aspnet/EntityFrameworkCore/issues/9033#issuecomment-317063370

            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var repository = services.GetRequiredService<IPortalRepository>();
                    repository.InitializeDatabase();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            return webHost;
        }
    }
}