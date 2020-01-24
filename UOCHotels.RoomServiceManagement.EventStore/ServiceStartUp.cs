using EventStore.ClientAPI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace UOCHotels.RoomServiceManagement.EventStore
{
    public class ServiceStartUp : IHostedService
    {
        private readonly IEventStoreConnection _esConnection;
        private readonly ProjectionManager _manager;
        private ILogger<ServiceStartUp> _logger;

        public ServiceStartUp(ILogger<ServiceStartUp> logger, IEventStoreConnection esConnection, ProjectionManager manager) { 
            _esConnection = esConnection;
            _manager = manager;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Information, "Starting EventStore subscription..");

            _esConnection.Connected += OnEventStoreConnected;
            _esConnection.Disconnected += OnEventStoreDisconnected;
            _esConnection.ErrorOccurred += OnEventStoreError;

            await _esConnection.ConnectAsync();

            _logger.Log(LogLevel.Information, "Initianting Projections manager..");

            _manager.Start();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _manager.Stop();
            _esConnection.Close();
        }

        private void OnEventStoreConnected(object sender, ClientConnectionEventArgs e)
        {
            _logger.LogInformation("EventStore connected.");
        }
        private void OnEventStoreDisconnected(object sender, ClientConnectionEventArgs e)
        {
            _logger.LogInformation("EventStore disconnected.");
        }

        private void OnEventStoreError(object sender, ClientErrorEventArgs e)
        {
            _logger.LogInformation($"EventStore error. {e.ToString()}");
        }
    }
}
