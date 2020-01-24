using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;
using UOCHotels.RoomServiceManagement.EventStore.Extensions;

namespace UOCHotels.RoomServiceManagement.EventStore
{
    public class AggregateStore : IAggregateStore
    {
        private readonly IEventStoreConnection _connection;
        public AggregateStore(IEventStoreConnection connection) => _connection = connection;
        public async Task<bool> Exists<TAggregate, TId>(TId aggregateId)
            where TAggregate : AggregateRoot<TId>
            where TId : ValueObject<TId>
        {
            var events = await this._connection.ReadStreamEventsForwardAsync(
                GetStreamName<TAggregate, TId>(aggregateId), 0, 1024, false);

            return events.Events.Any();
        }

        public async Task<AggregateRoot<TId>> Load<TAggregate, TId>(TId aggregate)
            where TAggregate : AggregateRoot<TId>
            where TId : ValueObject<TId>
        {
            var eventSlice = await this._connection.ReadStreamEventsForwardAsync(
               GetStreamName<TAggregate, TId>(aggregate), 0, 1024, false);

            //TODO paging
            TAggregate aggregateRoot = (TAggregate)Activator.CreateInstance(typeof(TAggregate), true);

            aggregateRoot.Load(eventSlice.Events.
                Select(resolvedEvent => resolvedEvent.Deserialize()).
                ToArray());

            return aggregateRoot;
        }

        public async Task Save<TAggregate, TId>(TAggregate aggregate)
            where TAggregate : AggregateRoot<TId>
            where TId : ValueObject<TId>
        {
            var changes = aggregate.GetChanges();

            if (!changes.Any())
                return;

            var events = changes.Select(@event =>
                                   new EventData(
                                           Guid.NewGuid(),
                                           @event.GetType().Name,
                                           true,
                                           Serialize(@event),
                                           Serialize(
                                               new EventMetadata
                                               {
                                                   ClrType = @event.GetType().AssemblyQualifiedName
                                               })));

            await this._connection.AppendToStreamAsync(
                    GetStreamName<TAggregate, TId>(aggregate),
                    aggregate.Version, events);

            aggregate.ClearChanges();
        }

        private static byte[] Serialize(object data) => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        private static string GetStreamName<T, TId>(TId aggregateId) => $"{typeof(T).Name}-{aggregateId.ToString()}";
        private static string GetStreamName<T, TId>(T aggregate) where T : AggregateRoot<TId>
            where TId : ValueObject<TId> => $"{typeof(T).Name}-{aggregate.Id.ToString()}";
    }

}
