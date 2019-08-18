using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;
using UOCHotels.RoomServiceManagement.Domain;
using UOCHotels.RoomServiceManagement.Domain.Infraestructure;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Persistence
{
    public class RoomServiceRepository : IRoomServiceRepository, IDisposable
    {
        private IAsyncDocumentSession _session;

        public RoomServiceRepository(IAsyncDocumentSession session)
        => _session = session;

        public Task<IEnumerable<RoomService>> GetByEmployeeId(EmployeeId id)
        {
            throw new NotImplementedException();
        }

        public Task Add(RoomService roomService)
        {
            return _session.StoreAsync(roomService, EntityId(roomService.Id));
        }

        public Task<RoomService> GetById(RoomServiceId id)
        {
            return _session.LoadAsync<RoomService>(EntityId(id));
        }

        public Task<IEnumerable<RoomService>> GetByRoomId(RoomId roomId)
        {
            throw new NotImplementedException();
        }

        public void Dispose() => _session.Dispose();

        private static string EntityId(RoomServiceId id)
            => $"RoomService/{id.ToString()}";
    }
}
