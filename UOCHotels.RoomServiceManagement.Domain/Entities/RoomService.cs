using System;
using UOCHotels.RoomServiceManagement.Domain.Enums;
using UOCHotels.RoomServiceManagement.Domain.Events;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;
using UOCHotels.RoomServiceManagement.Domain.Exceptions;
using System.Collections.Generic;

namespace UOCHotels.RoomServiceManagement.Domain
{
    public class RoomService : AggregateRoot<RoomServiceId>
    {
        private string DbId
        {
            get => $"RoomService/{Id.ToString()}";
            set { }
        }
        public RoomId AssociatedRoomId { get; private set; }
        public EmployeeId ServicedById { get; private set; }
        public RoomServiceStatus Status { get; private set; } = RoomServiceStatus.NotSet;
        public DateTime? PlannedOn { get; private set; }
        public DateTime? StartTimeStamp { get; private set; }
        public DateTime? EndTimeStamp { get; private set; }
        public List<Comment> Comments { get; private set; }

        internal RoomService(RoomServiceId id, RoomId roomId)
        {
            Id = id;
            AssociatedRoomId = roomId;
            Comments = new List<Comment>();
        }

        protected RoomService() { }

        public static RoomService Create(RoomServiceId serviceId, RoomId roomId)
        {
            var service = new RoomService(serviceId, roomId) { Status = RoomServiceStatus.Inactive };

            service.Apply(
                      new ServiceCreated(serviceId, roomId));

            return service;
        }

        public TimeSpan GetCompletionTimeDeviation(int calculatedDeviation)
        {
            if (this.Status != RoomServiceStatus.Completed)
            {
                throw new InvalidOperationException("Cannot estimate on a non-completed service.");
            }

            var actualDif = this.EndTimeStamp - this.StartTimeStamp;
            var estimatedDif = new TimeSpan(0, calculatedDeviation, 0);

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

        public void Start()
        {
            if (Status != RoomServiceStatus.Planned)
            {
                throw new InvalidRoomServiceOperationException("Can start only a planned room service.");

            }

            Apply(new ServiceStarted(Id, DateTime.UtcNow));
        }

        public void SubmitComment(string text, EmployeeId employeeId)
        {
            Apply(new CommentSubmitted(employeeId.GetValue(), Id.GetValue(), text));
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

                case ServiceFinished e:

                    EndTimeStamp = e.Timestamp;
                    Status = RoomServiceStatus.Completed;
                    break;

                case ServiceStarted e:

                    StartTimeStamp = e.StartTimestamp;
                    Status = RoomServiceStatus.Started;

                    break;
            }
        }

        public override void EnsureValidState()
        {
            if (this.Status == RoomServiceStatus.Inactive)
            {
            }
        }
    }
}
