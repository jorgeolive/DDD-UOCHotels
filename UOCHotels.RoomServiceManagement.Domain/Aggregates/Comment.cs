using System;
using UOCHotels.RoomServiceManagement.Domain.Events;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Aggregates
{
    public class Comment : Entity<CommentId>
    {
        public Comment(Action<object> applier) : base(applier) {}

        public string Text { get; internal set; }
        public EmployeeId CommentBy { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public override void EnsureValidState()
        {
            //throw new NotImplementedException();
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case CommentSubmitted e:

                    this.CommentBy = EmployeeId.CreateFor(e.SubmmitedBy);
                    Text = e.Text;
                    CreatedOn = DateTime.UtcNow;
                    this.Id = new CommentId(Guid.NewGuid());

                    break;
            } ;
        }
    }
}

