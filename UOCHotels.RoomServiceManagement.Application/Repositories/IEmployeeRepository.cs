using System.Collections.Generic;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Domain.Entities;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Repository
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetById(EmployeeId employeeId);
        Task Add(Employee employee);
        Task<List<Employee>> GetAll();
        Task Commit();
    }
}
