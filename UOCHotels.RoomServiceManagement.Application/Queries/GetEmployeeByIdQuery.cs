using System;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.ReadModel;

namespace UOCHotels.RoomServiceManagement.Application.Queries
{
    public class GetEmployeeByIdQuery : IRequest<EmployeeModel>
    {
        public Guid EmployeeId;

        public GetEmployeeByIdQuery(Guid employeeId) => EmployeeId = employeeId;
    }
}
