using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain.ValueObjects
{
    public class Comment : Entity<CommentId>
    {
        public Comment(Action<object> applier, CommentId id, string text, EmployeeId commentBy) : base(applier)
        {

            Text = string.IsNullOrEmpty(text) ? throw new ArgumentNullException(nameof(text)) : text;
            CommentBy = commentBy;
            CreatedOn = DateTime.Now;
            Id = id;
        }

        public string Text { get; internal set; }
        public EmployeeId CommentBy { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public override void EnsureValidState()
        {
            throw new NotImplementedException();
        }

        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }
    }
}

