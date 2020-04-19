using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NotesService.Core.Interfaces;
using NotesService.DataAccess;
using NotesService.DataAccess.Model;

[assembly: ApiController]
namespace NotesService.Api
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
            services.AddApplicationInsightsTelemetry();

            services.AddDbContext<NotesContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("NotesDb")));

            services.AddScoped<INoteRepository, NoteRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notes API", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalAndAppServiceAngular", builder =>
                    builder.WithOrigins("https://2002-ng-notes-client.azurewebsites.net",
                                        "http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                // remove the default text/plain string formatter to clean up the OpenAPI document
                options.OutputFormatters.RemoveType<StringOutputFormatter>();
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

            app.UseCors("AllowLocalAndAppServiceAngular");

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
