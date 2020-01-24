using System;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using UOCHotels.RoomServiceManagement.Application.Handlers.Commands;
using UOCHotels.RoomServiceManagement.Messaging.Configuration;
using UOCHotels.RoomServiceManagement.Messaging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UOCHotels.RoomServiceManagement.Application.Hubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Persistence.Configuration;
using UOCHotels.RoomServiceManagement.Persistence;
using UOCHotels.RoomServiceManagement.EventStore.Configuration;
using UOCHotels.RoomServiceManagement.EventStore;
using EventStore.ClientAPI;
using UOCHotels.RoomServiceManagement.Application.Projections;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide;
using UOCHotels.RoomServiceManagement.AzureStorage.Configuration;
using UOCHotels.RoomServiceManagement.Application.Services;
using UOCHotels.RoomServiceManagement.AzureStorage;

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
            services.Configure<EventStoreConfiguration>(options => Configuration.GetSection("EventStore").Bind(options));
            services.Configure<AzureStorageConfiguration>(options => Configuration.GetSection("AzureStorage").Bind(options));

            services.AddSingleton<RabbitMqSubscriberConfiguration>(
                provider =>
                provider.GetService<IOptions<RabbitMqSubscriberConfiguration>>().Value);

            services.AddSingleton<RavenDbConfiguration>(
                provider =>
                provider.GetService<IOptions<RavenDbConfiguration>>().Value);

            services.AddSingleton<EventStoreConfiguration>(
                provider =>
                provider.GetService<IOptions<EventStoreConfiguration>>().Value);

            services.AddSingleton<AzureStorageConfiguration>(
                provider =>
                provider.GetService<IOptions<AzureStorageConfiguration>>().Value);

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

            //var esConnection = EventStoreConnection.Create(Configuration["EventStore:ConnectionString"], ConnectionSettings.Create().KeepReconnecting());
            //var store = new AggregateStore(esConnection);

            services.AddSingleton<IEventStoreConnection>(
                provider =>
                {
                    return EventStoreConnection.
                        Create(
                            Configuration["EventStore:ConnectionString"],
                            ConnectionSettings.Create().KeepReconnecting());
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
                var ravenDb = new DocumentStore
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

                var db = ravenDb.Initialize();

                var record = ravenDb.Maintenance.Server.Send(
                new GetDatabaseRecordOperation(ravenDb.Database));

                if (record == null)
                {
                    ravenDb.Maintenance.Server.Send(
                        new CreateDatabaseOperation(new DatabaseRecord(ravenDb.Database)));
                }

                return db;
            });

            services.AddTransient<IRoomServiceRepository, RoomServiceRepository>();
            services.AddTransient<IRoomRepository, RoomRepository>();
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IRoomIncidentRepository, RoomIncidentRepository>();

            services.AddScoped<IFileStorageService, AzureStorageService>();

            services.AddSingleton<IProjection, RoomProjection>();
            services.AddSingleton<IProjection, RoomServiceProjection>();
            services.AddSingleton<IProjection, EmployeeProjection>();
            services.AddSingleton<IProjection, RoomIncidentProjection>();

            services.AddSingleton<IAggregateStore, AggregateStore>();
            services.AddSingleton<ProjectionManager>();

            services.AddMediatR(Assembly.GetAssembly(typeof(CreateRoomServiceCommandHandler)));
            services.AddHostedService<RabbitMqListener>();
            services.AddHostedService<ServiceStartUp>();
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
