using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using UOCHotels.RoomServiceManagement.Application.Hubs;
using UOCHotels.RoomServiceManagement.Application.ReadModel;
using UOCHotels.RoomServiceManagement.Domain.Events;
using UOCHotels.RoomServiceManagement.Application.Repositories;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Events
{
    public class RoomServiceCreatedEventHandler : INotificationHandler<ServiceCreated>
    {
        private readonly IHubContext<RoomServiceHub> _hubContext;

        private readonly IRoomRepository _roomRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRoomServiceRepository _roomServiceRepository;

        public RoomServiceCreatedEventHandler(IHubContext<RoomServiceHub> hubContext, IRoomRepository roomRepository, IEmployeeRepository employeeRepository, IRoomServiceRepository roomServiceRepository)
        {
            _hubContext = hubContext;
            _roomRepository = roomRepository;
            _employeeRepository = employeeRepository;
            _roomServiceRepository = roomServiceRepository;
        }

        public async Task Handle(ServiceCreated @event, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetById(@event.RoomId);
            var service = await _roomServiceRepository.GetById(@event.ServiceId);
            var employee = await _employeeRepository.GetById(@event.EmployeeId);

            if (room == null || service == null || employee == null)
                throw new ApplicationException();

            await _hubContext.Clients.All.SendAsync(
                "ReceiveMessage"
                , new RoomServiceModel()
                {
                    RoomNumber = room.Number,
                    Floor = room.Floor,
                    Owner = employee.Name,
                    PlannedOn = service.PlannedOn ?? null,
                    CompletedOn = service.CompletedOn ?? null,
                    Status = service.Status.ToString()
                });
        }
    }
}
