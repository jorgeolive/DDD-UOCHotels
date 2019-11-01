using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using UOCHotels.RoomServiceManagement.Domain.Infrastructure;
using UOCHotels.RoomServiceManagement.Persistence;
using Swashbuckle.AspNetCore.Swagger;
using UOCHotels.RoomServiceManagement.Application.Handlers.Commands;
using UOCHotels.RoomServiceManagement.Application.Services;
using UOCHotels.RoomServiceManagement.Messaging.Configuration;
using UOCHotels.RoomServiceManagement.Messaging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UOCHotels.RoomServiceManagement.Persistence.Configuration;
using UOCHotels.RoomServiceManagement.Application.Hubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

namespace RoomServiceManagement.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var cfgBuilder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

            Configuration = cfgBuilder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RabbitMqSubscriberConfiguration>(options => Configuration.GetSection("RabbitMQ").Bind(options));
            services.Configure<RavenDbConfiguration>(options => Configuration.GetSection("RavenDb").Bind(options));

            services.AddSingleton<RabbitMqSubscriberConfiguration>(
                provider =>
                provider.GetService<IOptions<RabbitMqSubscriberConfiguration>>().Value);

            services.AddSingleton<RavenDbConfiguration>(
                provider =>
                provider.GetService<IOptions<RavenDbConfiguration>>().Value);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
            AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = GetApiSecret(),
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

            services.AddCors(options =>
            {
                options.AddPolicy("SignalRClients",
                builder =>
                {
                    builder.WithOrigins("https://localhost:5001",
                                        "https://localhost:5000");
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "UOC RoomServiceManagement API",
                    Description = "Room Management Service Web API",
                    Contact = new OpenApiContact() { Name = "Jorge Olivé", Email = "j.olive.rodriguez@gmail.com" }
                });
            });

            services.AddSingleton(provider =>
            {
                var store = new DocumentStore
                {
                    Urls = new[] {
                        provider.GetService<RavenDbConfiguration>().Url
                    },
                    Database = provider.GetService<RavenDbConfiguration>().Database,
                    Conventions =
                    {
                        FindIdentityProperty = x => x.Name == "DbId"
                    }
                };

                return store.Initialize();
            });

            services.AddScoped<IRoomServiceRepository, RoomServiceRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddMediatR(Assembly.GetAssembly(typeof(CreateRoomServiceCommandHandler)));
            services.AddScoped<HouseKeepingPlanner>();
            services.AddHostedService<RabbitMqListener>();
            services.AddSignalR();
            services.AddMvcCore().AddApiExplorer();
            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("SignalRClients");

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            /*app.UseSignalR(routes =>
            {
                routes.MapHub<RoomServiceHub>("/RoomServiceHub");
            });*/

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<RoomServiceHub>("/RoomServiceHub");
            });
        }

        public SecurityKey GetApiSecret()
         =>
               //For future me: This is taken from environment variable :)
               string.IsNullOrEmpty(Configuration["AppSettings:JwtSecret"]) ?
               throw new ApplicationException("The API client secret is not setup")
               : new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Configuration["AppSettings:JwtSecret"]));
    }
}
