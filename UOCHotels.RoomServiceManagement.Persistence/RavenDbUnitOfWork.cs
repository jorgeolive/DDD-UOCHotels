using System;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;
using UOCHotels.RoomServiceManagement.Domain.Interfaces;

namespace UOCHotels.RoomServiceManagement.Persistence
{
    public class RavenDbUnitOfWork : IUnitOfWork
    {
        readonly IAsyncDocumentSession _documentSession;
        public RavenDbUnitOfWork(IAsyncDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public Task Commit()
        {
            return _documentSession.SaveChangesAsync();
        }
    }
}
