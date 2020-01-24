using System;
using System.Collections.Generic;
using UOCHotels.RoomServiceManagement.Domain.Enums;
using UOCHotels.RoomServiceManagement.Domain.Events;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Aggregates
{
    public class RoomIncident : AggregateRoot<RoomIncidentId>
    {
        public DateTime CreatedOn { get; internal set; }
        public RoomId AssociatedRoomId { get; internal set; }
        public string Comment { get; internal set; }
        public List<Uri> Pictures { get; internal set; }
        public Severity Severity { get; internal set; }

        public EmployeeId AssociatedEmployeId { get; internal set; }

        protected RoomIncident()
        {
            this.Pictures = new List<Uri>();
        }

        internal RoomIncident(EmployeeId employeeId, RoomId associatedRoomId, Severity severity, string comment, DateTime? createdOn)
        {
            Pictures = new List<Uri>();

            Apply(new RoomIncidentCreated(Guid.NewGuid(), employeeId.Value, associatedRoomId.Value, createdOn ?? DateTime.UtcNow, comment, severity));
        }

        public static RoomIncident CreateFor(EmployeeId employeeId, RoomId associatedRoomId, Severity severity, string comment, DateTime? createdOn)
        {
            return new RoomIncident(employeeId, associatedRoomId, severity, comment, createdOn);
        }

        public void AddPicture(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
                throw new ArgumentException("url has not valid value.");

            Apply(new PictureAdded(this.Id.Value, url.ToString()));
        }

        public override void EnsureValidState()
        {
            //throw new NotImplementedException();
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case PictureAdded e:

                    this.Pictures.Add(new Uri(e.Uri));
                    break;

                case RoomIncidentCreated e:
                    {
                        this.Id = RoomIncidentId.CreateFor(e.RoomIncidentId);
                        this.Comment = e.Comment;
                        this.AssociatedRoomId = RoomId.CreateFor(e.RoomId);
                        this.CreatedOn = e.CreatedOn;
                        this.Severity = e.Severity;
                        this.AssociatedEmployeId = EmployeeId.CreateFor(e.EmployeeId);
                    }
                    break;
            }
        }
    }
}
