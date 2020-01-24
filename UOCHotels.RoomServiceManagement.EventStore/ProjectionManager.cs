using EventStore.ClientAPI;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Application.Projections;
using UOCHotels.RoomServiceManagement.EventStore.Extensions;

namespace UOCHotels.RoomServiceManagement.EventStore
{
    public class ProjectionManager
    {
        readonly IEventStoreConnection storeConnection;
        readonly IProjection[] projections;
        private EventStoreAllCatchUpSubscription subscription;
        private readonly ILogger<ProjectionManager> _logger;

        public ProjectionManager(ILogger<ProjectionManager> logger, IEventStoreConnection connection, IEnumerable<IProjection> projections){
            this.projections = projections.ToArray();
            this.storeConnection = connection;
            _logger = logger;
        }

        public void Start()
        {          
            var settings = new CatchUpSubscriptionSettings(2000, 500, true, true, "try-out-subscription");
            subscription = storeConnection.SubscribeToAllFrom(
            Position.Start, settings, EventAppeared, LiveSubscriptionStarted, SubscriptionDropped);
        }

        private void SubscriptionDropped(EventStoreCatchUpSubscription _, SubscriptionDropReason dropReason, Exception e)
        {
            _logger.LogCritical("Catch-up subscription has been dropped!");
            _logger.LogCritical(dropReason.ToString());
            _logger.LogCritical(e.ToString());
        }

        private void LiveSubscriptionStarted(EventStoreCatchUpSubscription _)
        {
            _logger.LogInformation("All events processed.Started live subscription.");
        }

        public void Stop() => subscription.Stop();
        private Task EventAppeared(EventStoreCatchUpSubscription subscription,
        ResolvedEvent resolvedEvent)
        {         
            if (resolvedEvent.Event.EventType.StartsWith("$"))
                return Task.CompletedTask;
            var @event = resolvedEvent.Deserialize();

            _logger.LogInformation($"Processing event {resolvedEvent.ToString()}");
            _logger.LogInformation($"Last position processed {this.subscription.LastProcessedPosition.ToString()}");

            return Task.WhenAll(projections.Select(
            async x => await x.Project(@event)));
        }
    }
}
