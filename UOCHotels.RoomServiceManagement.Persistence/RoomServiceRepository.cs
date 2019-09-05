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
    public class RoomServiceRepository : RavenDbRepository<RoomService, RoomServiceId>, IRoomServiceRepository, IDisposable
    {
        private IAsyncDocumentSession _session;

        public RoomServiceRepository(IDocumentStore documentStore) : base(documentStore.OpenAsyncSession(), EntityId) => _session = documentStore.OpenAsyncSession();

        public Task<List<RoomService>> GetByEmployeeId(EmployeeId id)
         => _session.Query<RoomService>().Where(x => x.ServicedById == id).ToListAsync();

        public Task<List<RoomService>> GetByRoomId(RoomId roomId) =>
             _session.Query<RoomService>().Where(x => x.AssociatedRoomId == roomId).ToListAsync();

        public Task Commit()
         => _session.SaveChangesAsync();

        public void Dispose() => _session.Dispose();

        private static string EntityId(RoomServiceId id)
            => $"RoomService/{id.ToString()}";

    }
}
