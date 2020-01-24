using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using UOCHotels.RoomServiceManagement.Application.ReadModel;
using UOCHotels.RoomServiceManagement.Application.Repositories;

namespace UOCHotels.RoomServiceManagement.Persistence
{
    public class RoomIncidentRepository : RavenDbRepository<RoomIncidentModel>, IRoomIncidentRepository, IDisposable
    {
        public RoomIncidentRepository(IDocumentStore documentStore) : base(documentStore.OpenAsyncSession()) { }

        public Task<List<RoomIncidentModel>> GetByRoomId(Guid id)
        {
            return _session.Query<RoomIncidentModel>().Where(x => x.RoomId == id).ToListAsync();
        }
    }
}
