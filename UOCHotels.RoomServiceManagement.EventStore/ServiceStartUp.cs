using EventStore.ClientAPI;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace UOCHotels.RoomServiceManagement.EventStore
{
    public class ServiceStartUp : IHostedService
    {
        private readonly IEventStoreConnection _esConnection;
        public ServiceStartUp(IEventStoreConnection esConnection) { _esConnection = esConnection; }
        public Task StartAsync(CancellationToken cancellationToken) => _esConnection.ConnectAsync();
        public Task StopAsync(CancellationToken cancellationToken) { _esConnection.Close(); return Task.CompletedTask; }
    }
}
