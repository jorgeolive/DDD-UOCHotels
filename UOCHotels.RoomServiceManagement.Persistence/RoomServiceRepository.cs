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
        public RoomServiceRepository(IDocumentStore documentStore) : base(documentStore.OpenAsyncSession(), EntityId) { }

        public Task<List<RoomService>> GetByEmployeeId(EmployeeId id)
         => _session.Query<RoomService>().Where(x => x.ServicedById == id).ToListAsync();

        public Task<List<RoomService>> GetByRoomId(RoomId roomId) =>
             _session.Query<RoomService>().Where(x => x.AssociatedRoomId == roomId).ToListAsync();

        private static string EntityId(RoomServiceId id)
            => $"RoomService/{id.ToString()}";

    }
}
