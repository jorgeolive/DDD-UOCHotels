using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using UOCHotels.RoomServiceManagement.Domain;
using UOCHotels.RoomServiceManagement.Domain.Infraestructure;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Persistence
{
    public class RoomRepository : IRoomRepository
    {
        readonly IAsyncDocumentSession _session;

        public RoomRepository(IDocumentStore documentStore) => _session = documentStore.OpenAsyncSession();

        public Task<Room> GetById(RoomId roomId)
        {
            return _session.LoadAsync<Room>(EntityId(roomId));
        }

        public Task Add(Room room)
        {
            return _session.StoreAsync(room, EntityId(room.Id));
        }

        public Task Commit()
        {
            return _session.SaveChangesAsync();
        }

        private static string EntityId(RoomId id)
        => $"Room/{id.ToString()}";

        public Task<Room> GetByAddress(Address address)
        {
            return _session.Query<Room>().Where(x => x.Address == address).FirstOrDefaultAsync();
        }
    }
}
