using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Application.ReadModel;

namespace UOCHotels.RoomServiceManagement.Application.Repositories
{
    public interface IEmployeeRepository
    {
        Task<EmployeeModel> GetById(Guid employeeId);
        Task<bool> Exists(EmployeeModel readModel);
        Task Add(EmployeeModel employee);
        Task<List<EmployeeModel>> GetAll();
        Task Commit();
    }
}
