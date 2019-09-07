using System;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Persistence
{
    public abstract class RavenDbRepository<T, Tid> where T : AggregateRoot<Tid> where Tid : ValueObject<Tid>
    {
        protected RavenDbRepository(IAsyncDocumentSession session, Func<Tid, string> entityId)
        {
            _session = session;
            _entityId = entityId;
        }

        protected readonly IAsyncDocumentSession _session;
        readonly Func<Tid, string> _entityId;

        public Task<bool> Exists(Tid id) => _session.Advanced.ExistsAsync(_entityId(id));

        public Task<T> GetById(Tid id) => _session.LoadAsync<T>(_entityId(id));

        public Task Add(T entity) => _session.StoreAsync(entity, _entityId(entity.Id));

        public Task Commit() => _session.SaveChangesAsync();

        public void Dispose() => _session.Dispose();
    }
}
