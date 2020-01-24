using Raven.Client.Documents;
using UOCHotels.RoomServiceManagement.Application.ReadModel;
using UOCHotels.RoomServiceManagement.Application.Repositories;

namespace UOCHotels.RoomServiceManagement.Persistence
{
    public class EmployeeRepository : RavenDbRepository<EmployeeModel>, IEmployeeRepository
    {
        public EmployeeRepository(IDocumentStore documentStore) : base(documentStore.OpenAsyncSession()) { }
    }
}
