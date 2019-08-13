using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain.ValueObjects
{
    public class Comment : ValueObject<Comment>
    {
        private Comment() { }

        public string Text { get; internal set; }
        public EmployeeId CommentBy { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public static Comment Create(string text, Guid commentBy)
        {
            var comment = new Comment();

            comment.Text = string.IsNullOrEmpty(text) ? throw new ArgumentNullException(nameof(text)) : text;
            comment.CommentBy = new EmployeeId(commentBy);
            comment.CreatedOn = DateTime.Now;

            return comment;
        }

        protected override bool EqualsCore(Comment other)
        {
            return Text == other.Text && CommentBy == other.CommentBy && CreatedOn == other.CreatedOn;
        }
    }
}

