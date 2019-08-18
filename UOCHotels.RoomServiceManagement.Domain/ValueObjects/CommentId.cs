using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain.ValueObjects
{
    public class CommentId : ValueObject<CommentId>
    {
        private Guid _id;

        public CommentId(Guid id) =>
            _id = id == Guid.Empty ? throw new ArgumentNullException(nameof(id)) : id;

        public Guid GetValue() => _id;

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
