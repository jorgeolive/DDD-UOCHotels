using System;
using UOCHotels.RoomServiceManagement.Domain.Enums;
using UOCHotels.RoomServiceManagement.Domain.Events;
using UOCHotels.RoomServiceManagement.Domain.Exceptions;
using UOCHotels.RoomServiceManagement.Domain.Infrastructure;
using UOCHotels.RoomServiceManagement.Domain.Interfaces;

namespace UOCHotels.RoomServiceManagement.Domain
{
    public class RoomService : AggregateRoot
    {
        public Room Room { get; internal set; }
        public Employee ServicedBy { get; internal set; }
        public RoomServiceStatus Status { get; internal set; } = RoomServiceStatus.NotSet;
        public DateTime? PlannedOn { get; internal set; }
        public DateTime? StartTimeStamp { get; internal set; }
        public DateTime? EndTimeStamp { get; internal set; }

        protected RoomService(Guid id) : base(id) { }
        
        public static RoomService Create(Room room)
        {
            var service = new RoomService(Guid.NewGuid())
            {
                Room = room,
                Status = RoomServiceStatus.Inactive
            };

            return service;     
        }
        //Esto está todo mal... :( El aggregateRoot es la habitacion, por tanto estos métodos se deberian implementar en la clase Room.
        public void Plan(Employee employee, DateTime plannedOn)
        {
            if (this.Status != RoomServiceStatus.Inactive) throw new InvalidRoomServiceOperationException("Can't plan a Room Service that is not Inactive");
            Apply(new ServicePlanned()
            {
                Id = this.Id,
                EmployeeId = employee.Id,
                PlannedOn = plannedOn,
                RoomId = this.Room.Id
            });

            ServicedBy = employee;
            PlannedOn = plannedOn;
            Status = RoomServiceStatus.Planned;
        }

        public void Start()
        {
            if (Status != RoomServiceStatus.Planned) throw new InvalidRoomServiceOperationException("Can't start a non planned service");

            Status = RoomServiceStatus.Started;
            StartTimeStamp = DateTime.UtcNow;
        }

        public void Complete()
        {
            if (Status != RoomServiceStatus.Started) throw new InvalidRoomServiceOperationException("Only a started service can be completed.");

            Status = RoomServiceStatus.Completed;
            EndTimeStamp = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status == RoomServiceStatus.Completed) throw new InvalidRoomServiceOperationException("Can't cancel a completed service");
        }

        public TimeSpan GetCompletionTimeDeviation(IEstimateRoomServiceCalculator calculator)
        {
            if(this.Status != RoomServiceStatus.Completed)
            {
                throw new InvalidOperationException("Cannot estimate on a non-completed service.");
            }

            var actualDif = this.EndTimeStamp - this.StartTimeStamp;
            var estimatedDif = new TimeSpan(0, calculator.Calculate(this.Room, this.ServicedBy), 0);

            return actualDif.Value - estimatedDif;
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case ServicePlanned e:

                    ServicedBy = e.employee;
                    PlannedOn = plannedOn;
                    Status = RoomServiceStatus.Planned;

                    break;


            }
        }

        public override void EnsureValidState()
        {
            throw new NotImplementedException();
        }
    }
}
