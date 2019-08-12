using System;
using UOCHotels.RoomServiceManagement.Domain.Enums;
using UOCHotels.RoomServiceManagement.Domain.Events;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;
using UOCHotels.RoomServiceManagement.Domain.Interfaces;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;
using UOCHotels.RoomServiceManagement.Domain.Exceptions;

namespace UOCHotels.RoomServiceManagement.Domain
{
    public class RoomService : AggregateRoot<RoomServiceId>
    {
        public RoomId AssociatedRoomId { get; private set; }
        public EmployeeId ServicedById { get; private set; }
        public RoomServiceStatus Status { get; private set; } = RoomServiceStatus.NotSet;
        public DateTime? PlannedOn { get; private set; }
        public DateTime? StartTimeStamp { get; private set; }
        public DateTime? EndTimeStamp { get; private set; }

        protected RoomService(RoomServiceId id, RoomId roomId)
        {
            Id = id;
            AssociatedRoomId = roomId;
        }

        public static RoomService Create(RoomServiceId serviceId, RoomId associatedRoomId)
        {
            return new RoomService(serviceId, associatedRoomId)
            {
                AssociatedRoomId = associatedRoomId,
                Status = RoomServiceStatus.Inactive,
                Id = serviceId
            };
        }

        public TimeSpan GetCompletionTimeDeviation(IEstimateRoomServiceCalculator calculator)
        {
            if (this.Status != RoomServiceStatus.Completed)
            {
                throw new InvalidOperationException("Cannot estimate on a non-completed service.");
            }

            var actualDif = this.EndTimeStamp - this.StartTimeStamp;
            var estimatedDif = new TimeSpan(0, calculator.Calculate(AssociatedRoomId, ServicedById), 0);

            return actualDif.Value - estimatedDif;
        }

        public void Plan(DateTime timeStamp, EmployeeId employeeId)
        {
            if (Status != RoomServiceStatus.Inactive)
            {
                throw new InvalidRoomServiceOperationException("Room service is not started");
            }

            Apply(
            new ServicePlanned(this.Id, employeeId, timeStamp));
        }

        public void Complete()
        {
            if (Status != RoomServiceStatus.Started)
            {
                throw new InvalidRoomServiceOperationException("Room service is not started");
            }

            Apply(new ServiceFinished(Id, DateTime.Now));
        }


        protected override void When(object @event)
        {
            switch (@event)
            {
                case ServicePlanned e:

                    ServicedById = new EmployeeId(e.ServiceOwnerId);
                    PlannedOn = e.PlannedOn;
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
