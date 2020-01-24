using System;
using System.Collections.Generic;
using System.Security.Claims;
using UOCHotels.RoomServiceManagement.Domain.Enums;
using UOCHotels.RoomServiceManagement.Domain.Events;
using UOCHotels.RoomServiceManagement.Domain.Exceptions;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Aggregates
{
    public class RoomService : AggregateRoot<RoomServiceId>
    {
        public RoomServiceType ServiceType { get; private set; }
        public RoomId AssociatedRoomId { get; private set; }
        public EmployeeId ServicedById { get; private set; }
        public RoomServiceStatus Status { get; private set; } = RoomServiceStatus.Inactive;
        public DateTime? PlannedOn { get; private set; }
        public DateTime? StartTimeStamp { get; private set; }
        public DateTime? EndTimeStamp { get; private set; }
        public List<Comment> Comments { get; } = new List<Comment>();

        private RoomService(RoomServiceId id, RoomId roomId, EmployeeId employeeId)
        {
            Apply(new ServiceCreated(DateTime.UtcNow, id.Value, roomId.Value, employeeId.Value));
        }

        protected RoomService(){}

        public static RoomService CreateFor(RoomId roomId, EmployeeId employeeId)
        {
            if (roomId == null || roomId.Value == default)
                throw new InvalidRoomServiceOperationException("roomId cannot be null.");

            if (employeeId == null || employeeId.Value == default)
                throw new InvalidRoomServiceOperationException("employeeId cannot be null.");

            return new RoomService(RoomServiceId.CreateFor(Guid.NewGuid()), roomId, employeeId);
        }

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

            Apply(new ServicePlanned(Id.Value, timeStamp));
        }

        public void Complete()
        {
            if (Status != RoomServiceStatus.Started)
            {
                throw new InvalidRoomServiceOperationException("Room service is not started");
            }

            Apply(new ServiceFinished(Id.Value, DateTime.Now));
        }

        public void Start()
        {
            if (Status != RoomServiceStatus.Planned)
            {
                throw new InvalidRoomServiceOperationException("Can start only a planned room service.");
            }

            Apply(new ServiceStarted(Id.Value, DateTime.UtcNow));
        }

        public void AddComment(string text, EmployeeId addedBy)
        {
            if(string.IsNullOrWhiteSpace(text)) 
                throw new ArgumentNullException(nameof(text));

            Apply(new CommentSubmitted(Guid.NewGuid(),addedBy.Value, this.Id.Value, text));
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case ServiceCreated e:

                    Id = new RoomServiceId(e.ServiceId);
                    AssociatedRoomId = new RoomId(e.RoomId);
                    ServicedById = EmployeeId.CreateFor(e.EmployeeId);
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
                    ApplyToEntity(comment, e); //
                    this.Comments.Add(comment);
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
