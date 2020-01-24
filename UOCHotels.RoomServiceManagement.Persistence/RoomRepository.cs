using System;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using UOCHotels.RoomServiceManagement.Application.ReadModel;
using UOCHotels.RoomServiceManagement.Application.Repositories;

namespace UOCHotels.RoomServiceManagement.Persistence
{
    public class RoomRepository : RavenDbRepository<RoomModel>, IRoomRepository, IDisposable
    {
        public RoomRepository(IDocumentStore documentStore) : base(documentStore.OpenAsyncSession()) { }

        public Task<RoomModel> GetByAddress(string building, int floor, int number)
        {
            return _session.Query<RoomModel>().Where(x => x.Floor == floor && x.Building == building && x.Number == number).FirstOrDefaultAsync();
        }
    }
}
