using System;
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

        private static string EntityId(RoomId id)
        => $"RoomService/{id.ToString()}";
    }
}
