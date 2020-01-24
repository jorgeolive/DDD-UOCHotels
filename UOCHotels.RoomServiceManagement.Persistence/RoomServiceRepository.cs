using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using UOCHotels.RoomServiceManagement.Application.ReadModel;
using UOCHotels.RoomServiceManagement.Application.Repositories;

namespace UOCHotels.RoomServiceManagement.Persistence
{
    public class RoomServiceRepository : RavenDbRepository<RoomServiceModel>, IRoomServiceRepository, IDisposable
    {
        public RoomServiceRepository(IDocumentStore documentStore) : base(documentStore.OpenAsyncSession()) { }

        public Task<List<RoomServiceModel>> GetByEmployeeId(Guid id)
         => _session.Query<RoomServiceModel>().Where(x => x.EmployeeId == id).ToListAsync();
    
        public Task<List<RoomServiceModel>> GetByRoomId(Guid roomId) =>
             _session.Query<RoomServiceModel>().Where(x => x.RoomId == roomId).ToListAsync();

    }
}
