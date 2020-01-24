using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Domain.Aggregates;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Commands
{
    public class AddRoomIncidentRequestHandler : AsyncRequestHandler<AddRoomIncidentRequest>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAggregateStore _store;
        public AddRoomIncidentRequestHandler(IAggregateStore store, IRoomRepository roomRepository, IEmployeeRepository employeeRepository)
        {
            _roomRepository = roomRepository;
            _employeeRepository = employeeRepository;
            _store = store;
        }

        protected override async Task Handle(AddRoomIncidentRequest request, CancellationToken cancellationToken)
        {
            if ((await _roomRepository.GetById(request.RoomId)) != null &&
                (await _employeeRepository.GetById(request.EmployeeId)) != null)
            {
                var roomIncident = RoomIncident.CreateFor(
                    EmployeeId.CreateFor(request.EmployeeId),
                    RoomId.CreateFor(request.RoomId),
                    request.Severity, 
                    request.Comment, 
                    request.CreatedOn);

                await _store.Save<RoomIncident, RoomIncidentId>(roomIncident);
            }
            else
            {
                throw new InvalidOperationException("Room or employee doesnt exist");
            }         
        }
    }
}
