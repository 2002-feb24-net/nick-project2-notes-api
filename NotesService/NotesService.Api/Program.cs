using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotesService.DataAccess.Model;

namespace NotesService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // standard Main method altered here to run database seeding in between Build() and Run(),
            // if runtime configuration says to ("EnsureDatabaseCreated" set to true).
            // this is needed if the DB server is running in a new container.

            // setting up the database during app startup like this is a bad practice for several reasons:
            // - concurrency problems with multiple instances of the app
            // - requires schema-modifying privileges on the login used by the app to connect
            // but it's good enough for dev scenarios.

            IHost host = CreateHostBuilder(args).Build();

            using (IServiceScope scope = host.Services.CreateScope())
            {
                IServiceProvider serviceProvider = scope.ServiceProvider;
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var logger = serviceProvider.GetService<ILogger<Program>>();

                if (configuration.GetValue("EnsureDatabaseCreated", defaultValue: false) is true)
                {
                    logger.LogInformation("Ensuring database is created...");
                    try
                    {
                        var dbContext = serviceProvider.GetRequiredService<NotesContext>();
                        dbContext.Database.EnsureCreated();
                    }
                    catch (Exception ex)
                    {
                        logger.LogCritical("Error while ensuring database is created.", ex);
                        throw;
                    }
                    logger.LogInformation("Ensured database is created.");
                }
                else
                {
                    logger.LogInformation("Not ensuring database is created.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
