using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Exceptions;
using UOCHotels.RoomServiceManagement.Domain.Entities;
using UOCHotels.RoomServiceManagement.Domain.Infrastructure;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Commands
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
            RoomService roomService = null;

            try
            {
                roomService = await this._roomServiceRepository.GetById(new RoomServiceId(request.RoomServiceId));
            }
            catch (Exception e)
            {
                throw new RoomServiceNotFoundException(e.Message);
            }

            roomService.Plan(request.PlanDate);
            await _roomServiceRepository.Commit();
            //Here we might want to notify other services through the messaging queue. 
        }
    }
}
