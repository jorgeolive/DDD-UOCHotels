using System;
using UOCHotels.RoomServiceManagement.Domain.Events;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Entities
{
    public class Comment : Entity<CommentId>
    {
        private string DbId
        {
            get => $"Comment/{Id.ToString()}";
            set { }
        }

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

                    this.CommentBy = new EmployeeId(e.SubmmitedBy);
                    Text = e.Text;
                    CreatedOn = DateTime.UtcNow;

                    break;
            } ;
        }
    }
}

