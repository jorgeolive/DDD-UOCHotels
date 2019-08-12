using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain.ValueObjects
{

    public class DoorNumber : ValueObject<DoorNumber>
    {
        private int _doorNumber;

        public static DoorNumber CreateFor(int doorNumber)
        {
            return new DoorNumber(doorNumber);
        }

        private DoorNumber(int doorNumber)
        {
            _doorNumber = doorNumber;
        }

        protected override bool EqualsCore(DoorNumber other)
        {
            return _doorNumber == other._doorNumber;
        }

        public override string ToString()
        {
            return _doorNumber.ToString();
        }
    }
}