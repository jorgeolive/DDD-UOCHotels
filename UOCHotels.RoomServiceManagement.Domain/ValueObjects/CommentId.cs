using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain.ValueObjects
{
    public class CommentId : ValueObject<CommentId>
    {
        public Guid Value;

        internal CommentId() { }

        public CommentId(Guid id) =>
            Value = id == Guid.Empty ? throw new ArgumentNullException(nameof(id)) : id;

        public Guid GetValue() => Value;

        protected override bool EqualsCore(CommentId other)
        {
            return this == other;
        }

        public static bool operator ==(CommentId id1, CommentId id2)
        {
            return id1.GetValue() == id2.GetValue();
        }

        public static bool operator !=(CommentId id1, CommentId id2)
        {
            return id1.GetValue() != id2.GetValue();
        }
    }
}
