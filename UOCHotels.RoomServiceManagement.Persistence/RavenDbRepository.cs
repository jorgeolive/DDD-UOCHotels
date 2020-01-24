using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using UOCHotels.RoomServiceManagement.Application.ReadModel;

namespace UOCHotels.RoomServiceManagement.Persistence
{
    public abstract class RavenDbRepository<T> where T : IReadModel
    {
        protected RavenDbRepository(IAsyncDocumentSession session)
        {
            _session = session;
        }

        protected readonly IAsyncDocumentSession _session;

        public Task<bool> Exists(T readModel) => _session.Advanced.ExistsAsync(readModel.GetId());

        public Task<T> GetById(Guid id) => _session.LoadAsync<T>(id.ToString());

        public Task Add(T entity) => _session.StoreAsync(entity, entity.GetId());

        public void Dispose() => _session.Dispose();

        public Task<List<T>> GetAll() => _session.Query<T>().ToListAsync();

        public Task Commit() => _session.SaveChangesAsync();
    }
}
