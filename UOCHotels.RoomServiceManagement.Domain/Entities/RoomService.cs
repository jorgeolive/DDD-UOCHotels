using System;
using System.Collections.Generic;
using UOCHotels.RoomServiceManagement.Domain.Enums;
using UOCHotels.RoomServiceManagement.Domain.Events;
using UOCHotels.RoomServiceManagement.Domain.Exceptions;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Entities
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
        public RoomServiceStatus Status { get; private set; } = RoomServiceStatus.Inactive;
        public DateTime? PlannedOn { get; private set; }
        public DateTime? StartTimeStamp { get; private set; }
        public DateTime? EndTimeStamp { get; private set; }
        public List<Comment> Comments { get; } = new List<Comment>();

        public RoomService(RoomServiceId id, RoomId roomId, EmployeeId employeeId)
        {
            Apply(new ServiceCreated(id, roomId, employeeId));
        }

        protected RoomService(){ }

        public TimeSpan GetCompletionTimeDeviation(int calculatedDeviation)
        {
            if (this.Status != RoomServiceStatus.Completed)
            {
                throw new InvalidOperationException("Cannot estimate on a non-completed service.");
            }

            var actualDif = this.EndTimeStamp - this.StartTimeStamp;
            var estimatedDif = new TimeSpan(0, calculatedDeviation, 0);

            if (actualDif != null) return actualDif.Value - estimatedDif;

            return new TimeSpan();
        }

        public void Plan(DateTime timeStamp)
        {
            if (Status != RoomServiceStatus.Inactive)
            {
                throw new InvalidRoomServiceOperationException("Room service is not started");
            }

            Apply(
            new ServicePlanned(this.Id, timeStamp));
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

        public void AddComment(string text, EmployeeId addedBy)
        {
            if(string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));
            Apply(new CommentSubmitted(addedBy.Value, this.Id.Value, text));
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case ServiceCreated e:

                    Id = new RoomServiceId(e.ServiceId);
                    AssociatedRoomId = new RoomId(e.RoomId);
                    ServicedById = new EmployeeId(e.EmployeeId);

                    break;

                case ServicePlanned e:

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

                case CommentSubmitted e:
                
                    var comment = new Comment(Apply);
                    ApplyToEntity(comment, e);
                
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
