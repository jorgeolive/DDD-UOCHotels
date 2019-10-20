using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Domain.Entities;
using UOCHotels.RoomServiceManagement.Domain.Infrastructure;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Commands
{
    public class AddEmployeeRequestHandler : AsyncRequestHandler<AddEmployeeRequest>
    {
        readonly IMediator _mediator;
        readonly IEmployeeRepository _employeeRepository;

        public AddEmployeeRequestHandler(
                            IMediator mediator,
                            IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(AddEmployeeRequest request, CancellationToken cancellationToken)
        {
            await _employeeRepository.Add(Employee.Create(request.OnBoardingDate, request.Name, request.SurName));
            await _employeeRepository.Commit();
        }
    }
}
