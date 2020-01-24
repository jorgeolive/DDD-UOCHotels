using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class CommentSubmitted : INotification
    {
        public Guid SubmmitedBy { get; internal set; }
        public string Text { get; internal set; }
        public Guid RoomServiceId { get; internal set; }
        public Guid CommentId { get; internal set; }
        public CommentSubmitted(Guid commentId, Guid submittedBy, Guid roomServiceId, string text)
        {
            Text = text;
            RoomServiceId = roomServiceId;
            SubmmitedBy = submittedBy;
            CommentId = commentId;
        }
    }
}
