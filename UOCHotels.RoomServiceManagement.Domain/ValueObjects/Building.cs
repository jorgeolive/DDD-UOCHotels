using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain
{
    public class Building : ValueObject<Building>
    {
        public string Name { get; internal set; }

        public static Building CreateFor(string buildingName)
        {
            return string.IsNullOrEmpty(buildingName) ? throw new ArgumentNullException(nameof(buildingName)) : new Building(buildingName);
        }

        private Building(string buildingName)
        {
            Name = buildingName;
        }

        public override string ToString()
        {
            return Name;
        }

        protected override bool EqualsCore(Building other)
        {
            return Name == other.Name;
        }
    }
}