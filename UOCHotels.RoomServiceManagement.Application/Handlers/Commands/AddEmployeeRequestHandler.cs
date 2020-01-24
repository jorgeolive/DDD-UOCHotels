using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Domain.Aggregates;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;
using System.Linq;
using System;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Commands
{
    public class AddEmployeeRequestHandler : AsyncRequestHandler<AddEmployeeRequest>
    {
        readonly IMediator _mediator;
        readonly IEmployeeRepository _employeeRepository;
        private readonly IAggregateStore eventStore;

        public AddEmployeeRequestHandler(
                            IMediator mediator,
                            IEmployeeRepository employeeRepository,
                            IAggregateStore eventStore)
        {
            _employeeRepository = employeeRepository;
            _mediator = mediator;
            this.eventStore = eventStore;
        }

        protected override async Task Handle(AddEmployeeRequest request, CancellationToken cancellationToken)
        {
            //To be refactored.
            if (!(await _employeeRepository.GetAll()).Where(x => x.SocialSecurityNumber == request.SocialSecurityNumber).Any())
            {
                var employee = Employee.Create(request.Name, request.SurName, request.OnBoardingDate, request.SocialSecurityNumber, request.DateOfBirth);
                await this.eventStore.Save<Employee, EmployeeId>(employee);
            }
            else
            {
                throw new Exception("Employee already exists.");
            }
        }
    }
}
