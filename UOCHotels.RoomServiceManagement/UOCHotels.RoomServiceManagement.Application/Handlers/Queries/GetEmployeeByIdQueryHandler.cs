using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Queries;
using UOCHotels.RoomServiceManagement.Application.ReadModel;
using UOCHotels.RoomServiceManagement.Domain.Infrastructure;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Queries
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeModel>
    {
        readonly IEmployeeRepository _employeeRepository;

        public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeModel> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetById(new Domain.ValueObjects.EmployeeId(request.EmployeeId));

            return new EmployeeModel()
            {
                Name = employee.Name,
                SurName = employee.SurName,
                OnBoardDate = employee.OnboardingDate
            };
        }
    }
}
