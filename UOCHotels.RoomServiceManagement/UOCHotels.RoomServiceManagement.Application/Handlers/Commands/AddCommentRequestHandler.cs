using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Exceptions;
using UOCHotels.RoomServiceManagement.Domain.Infrastructure;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Commands
{
    public class AddCommentRequestHandler : AsyncRequestHandler<AddCommentRequest>
    {
        readonly IMediator _mediator;
        readonly IRoomServiceRepository _roomServiceRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public AddCommentRequestHandler(IMediator mediator, IRoomServiceRepository roomServiceRepository,
            IEmployeeRepository employeeRepository)
        {
            _roomServiceRepository = roomServiceRepository;
            _employeeRepository = employeeRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(AddCommentRequest request, CancellationToken cancellationToken)
        {
            var roomService = await _roomServiceRepository.GetById(new RoomServiceId(request.RoomServiceId));

            if (roomService == null) throw new RoomServiceNotFoundException(request.RoomServiceId.ToString());

            roomService.AddComment(request.Text, new EmployeeId(request.EmployeeId));
            await _roomServiceRepository.Commit();
        }
    }
}
