using System;
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
        readonly IAsyncDocumentSession _session;

        public RoomRepository(IDocumentStore documentStore) : base(documentStore.OpenAsyncSession(), EntityId) => _session = documentStore.OpenAsyncSession();

        public Task Commit()
        {
            return _session.SaveChangesAsync();
        }

        protected static string EntityId(RoomId id)
        => $"Room/{id.ToString()}";

        public Task<Room> GetByAddress(Address address)
        {
            return _session.Query<Room>().Where(x => x.Address == address).FirstOrDefaultAsync();
        }

        public void Dispose() => _session.Dispose();
    }
}
