using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NotesService.Core.Interfaces;
using NotesService.DataAccess;
using NotesService.DataAccess.Model;

[assembly: ApiController]
namespace NotesService.Api
{
    public class Startup
    {
        private const string SqlServerConnection = "NotesDb";
        private const string PostgreSqlConnection = "NotesDbPostgreSql";
        private const string CorsPolicyName = "AllowConfiguredOrigins";

        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            // switch between database providers using runtime configuration
            // (the existing migrations are SQL-Server-specific, but the model itself is not)

            // this should be the name of a connection string.
            string whichDb = Configuration["DatabaseConnection"];
            if (whichDb is null)
            {
                _logger.LogWarning($"No configuration found for \"DatabaseConnection\"; assuming default \"{SqlServerConnection}\".");
                whichDb = SqlServerConnection;
            }

            string connection = Configuration.GetConnectionString(whichDb);
            if (connection is null)
            {
                string message = $"No value found for \"{whichDb}\" connection; unable to connect to a database.";
                _logger.LogCritical(message);
                throw new InvalidOperationException(message);
            }

            _logger.LogInformation($"Using \"{whichDb}\" connection.");

            switch (whichDb)
            {
                case PostgreSqlConnection:
                    _logger.LogInformation($"Using PostgreSQL.");
                    services.AddDbContext<NotesContext>(options =>
                        options.UseNpgsql(connection));
                    break;
                case SqlServerConnection:
                    _logger.LogInformation($"Using SQL Server.");
                    services.AddDbContext<NotesContext>(options =>
                        options.UseSqlServer(connection));
                    break;
                default:
                    _logger.LogWarning($"Unexpected connection \"{whichDb}\" assumed to be SQL Server.");
                    services.AddDbContext<NotesContext>(options =>
                        options.UseSqlServer(connection));
                    break;
            }

            services.AddScoped<INoteRepository, NoteRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notes API", Version = "v1" });
            });

            // support switching between database providers using runtime configuration

            var allowedOrigins = Configuration.GetSection("CorsOrigins").Get<string[]>();
            if (allowedOrigins?.Length > 0)
            {
                _logger.LogInformation($"Origins allowed by CORS policy: {string.Join(", ", allowedOrigins.Select(x => $"\"{x}\""))}");
            }
            else
            {
                _logger.LogError("No origins allowed for CORS.");
                allowedOrigins = Array.Empty<string>();
            }

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, builder =>
                    builder.WithOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddControllers(options =>
            {
                // remove the default text/plain string formatter to clean up the OpenAPI document
                options.OutputFormatters.RemoveType<StringOutputFormatter>();

                options.ReturnHttpNotAcceptable = true;
                options.SuppressAsyncSuffixInActionNames = false;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CorsPolicyName);

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notes API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
