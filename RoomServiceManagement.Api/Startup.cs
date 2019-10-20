using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Domain.Infrastructure;
using UOCHotels.RoomServiceManagement.Persistence;
using Swashbuckle.AspNetCore.Swagger;
using UOCHotels.RoomServiceManagement.Application.Handlers;
using UOCHotels.RoomServiceManagement.Application.Handlers.Commands;
using UOCHotels.RoomServiceManagement.Application.Services;
using UOCHotels.RoomServiceManagement.Application.HostedServices;
using UOCHotels.RoomServiceManagement.Messaging.Configuration;
using UOCHotels.RoomServiceManagement.Messaging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UOCHotels.RoomServiceManagement.Persistence.Configuration;

namespace RoomServiceManagement.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "UOC RoomServiceManagement API",
                    Description = "Room Management Service Web API",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Jorge Olivé", Email = "j.olive.rodriguez@gmail.com" }
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
            //services.AddHostedService<HouseKeepingPlannerService>();
            services.AddHostedService<RabbitMqListener>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
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
