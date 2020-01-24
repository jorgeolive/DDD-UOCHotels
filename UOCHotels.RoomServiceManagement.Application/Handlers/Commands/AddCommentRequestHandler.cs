using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Exceptions;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Domain.Aggregates;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Commands
{
    public class AddCommentRequestHandler : AsyncRequestHandler<AddCommentRequest>
    {
        private readonly IAggregateStore store;
        private readonly IRoomServiceRepository _roomServiceRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public AddCommentRequestHandler(IAggregateStore store, IRoomServiceRepository roomServiceRepository,
            IEmployeeRepository employeeRepository)
        {
            this.store = store;
            _roomServiceRepository = roomServiceRepository;
            _employeeRepository = employeeRepository;
        }

        protected override async Task Handle(AddCommentRequest request, CancellationToken cancellationToken)
        {
            if (!(await store.Load<RoomService, RoomServiceId>(RoomServiceId.CreateFor(request.RoomServiceId)) is RoomService roomService))
                throw new RoomServiceNotFoundException(request.RoomServiceId.ToString());

            roomService.AddComment(request.Text, EmployeeId.CreateFor(request.EmployeeId));

            await store.Save<RoomService, RoomServiceId>(roomService);
        }
    }
}
