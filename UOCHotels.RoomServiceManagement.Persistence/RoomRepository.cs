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
    public class RoomRepository : RavenDbRepository<Room, RoomId>, IRoomRepository, IDisposable
    {
        public RoomRepository(IDocumentStore documentStore) : base(documentStore.OpenAsyncSession(), EntityId) { }


        protected static string EntityId(RoomId id)
        => $"Room/{id.ToString()}";

        public Task<Room> GetByAddress(Address address)
        {
            return _session.Query<Room>().Where(x => x.Address == address).FirstOrDefaultAsync();
        }

        public Task<List<Room>> GetAll() => _session.Query<Room>().ToListAsync();

    }
}
