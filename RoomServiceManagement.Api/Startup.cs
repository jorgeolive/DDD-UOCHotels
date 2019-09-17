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
using UOCHotels.RoomServiceManagement.Domain.Infraestructure;
using UOCHotels.RoomServiceManagement.Persistence;
using Swashbuckle.AspNetCore.Swagger;
using UOCHotels.RoomServiceManagement.Application.Handlers;
using UOCHotels.RoomServiceManagement.Application.Services;
using UOCHotels.RoomServiceManagement.Application.HostedServices;
using UOCHotels.RoomServiceManagement.Messaging.Configuration;
using UOCHotels.RoomServiceManagement.Messaging;

namespace RoomServiceManagement.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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

            //TODO OBTAIN THE DB PARAMS FROM CONFIG ;)
            services.AddSingleton(provider =>
            {
                var store = new DocumentStore
                {
                    Urls = new[] {
                        "http://host.docker.internal:8080"
                    },
                    Database = "RoomServiceManagement",
                    Conventions =
                    {
                        FindIdentityProperty = x => x.Name == "DbId"
                    }
                };

                return store.Initialize();
            });

            var rabbitMQconfig = new RabbitMqSubscriberConfiguration();
            Configuration.Bind("RabbitMQ", rabbitMQconfig);
            services.AddSingleton(rabbitMQconfig);

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
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
