using System;
using Raven.Client.Documents;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Domain;
using UOCHotels.RoomServiceManagement.Domain.Entities;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Persistence
{
    public class EmployeeRepository : RavenDbRepository<Employee, EmployeeId>, IEmployeeRepository
    {
        public EmployeeRepository(IDocumentStore documentStore) : base(documentStore.OpenAsyncSession(), EntityId) { }

        protected static string EntityId(EmployeeId id)
       => $"Employee/{id.ToString()}";
    }
}
