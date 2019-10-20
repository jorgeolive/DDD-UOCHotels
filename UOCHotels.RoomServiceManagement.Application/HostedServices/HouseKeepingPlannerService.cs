using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UOCHotels.RoomServiceManagement.Application.Services;

namespace UOCHotels.RoomServiceManagement.Application.HostedServices
{
    public class HouseKeepingPlannerService : IHostedService
    {
        public IServiceProvider Services { get; }

        public HouseKeepingPlannerService(IServiceProvider serviceProvider) => Services = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                using (var scope = Services.CreateScope())
                {
                    var houseKeepingPlanner =
                      scope.ServiceProvider
                      .GetRequiredService<HouseKeepingPlanner>();

                    await houseKeepingPlanner.Execute(null);
                    Thread.Sleep(1000);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
