using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Exceptions;
using UOCHotels.RoomServiceManagement.Domain;
using UOCHotels.RoomServiceManagement.Domain.Infraestructure;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Handlers
{
    public class PlanRoomServiceRequestHandler : AsyncRequestHandler<PlanRoomServiceRequest>
    {
        readonly IEmployeeRepository _employeeRepository;
        readonly IRoomServiceRepository _roomServiceRepository;
        readonly IMediator _mediator;

        public PlanRoomServiceRequestHandler(
            IEmployeeRepository employeeRepository,
            IRoomServiceRepository roomServiceRepository,
            IMediator mediator)
        {
            _roomServiceRepository = roomServiceRepository;
            _employeeRepository = employeeRepository;
            this._mediator = mediator;
        }

        protected override async Task Handle(PlanRoomServiceRequest request, CancellationToken cancellationToken)
        {
            RoomService room = null;

            try
            {
                room = await this._roomServiceRepository.GetById(new RoomServiceId(request.RoomServiceId));
            }
            catch (Exception e)
            {
                throw new RoomServiceNotFoundException(e.Message);
            }

            Employee employee = null;

            try
            {
                employee = await this._employeeRepository.GetById(new EmployeeId(request.EmployeeId));
            }
            catch
            {
                throw new RoomServiceNotFoundException("");
            }

            room.Plan(request.PlanDate, employee.Id);
            await _roomServiceRepository.Commit();
            //Here we might want to notify other services through the messaging queue. 
        }
    }
}
