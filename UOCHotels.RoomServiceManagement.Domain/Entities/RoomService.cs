using System;
using UOCHotels.RoomServiceManagement.Domain.Enums;
using UOCHotels.RoomServiceManagement.Domain.Events;
using UOCHotels.RoomServiceManagement.Domain.Exceptions;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;
using UOCHotels.RoomServiceManagement.Domain.Interfaces;

namespace UOCHotels.RoomServiceManagement.Domain
{
    public class RoomService : Entity
    {
        public Room Room { get; private set; }
        public Employee ServicedBy { get; private set; }
        public RoomServiceStatus Status { get; private set; } = RoomServiceStatus.NotSet;
        public DateTime? PlannedOn { get; private set; }
        public DateTime? StartTimeStamp { get; private set; }
        public DateTime? EndTimeStamp { get; private set; }

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

        public TimeSpan GetCompletionTimeDeviation(IEstimateRoomServiceCalculator calculator)
        {
            if (this.Status != RoomServiceStatus.Completed)
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
