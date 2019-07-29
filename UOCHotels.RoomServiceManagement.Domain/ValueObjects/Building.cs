using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain
{
    public class Building : ValueObject<Building>
    {
        public string Name { get; internal set; }

        public Building(string buildingName)
        {
            Name = string.IsNullOrEmpty(buildingName) ? throw new ArgumentNullException(nameof(buildingName)) : buildingName;
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