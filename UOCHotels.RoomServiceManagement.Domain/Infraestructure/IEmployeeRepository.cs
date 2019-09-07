using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Infraestructure
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetById(EmployeeId employeeId);
        Task Add(Employee employee);
        Task Commit();
    }
}
