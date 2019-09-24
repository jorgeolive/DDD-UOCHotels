using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Entities
{
    public class Employee : AggregateRoot<EmployeeId>
    {
        public string Name { get; internal set; }
        public string SurName { get; internal set; }
        public DateTime OnboardingDate { get; internal set; }
        public bool OnLeave { get; internal set; } = false;
        public int ExperienceMonths => Math.Abs((DateTime.UtcNow.Month - OnboardingDate.Date.Month) + 12 * (DateTime.UtcNow.Year - OnboardingDate.Date.Year));

        private string DbId
        {
            get => $"Employee/{Id.ToString()}";
            set { }
        }

        private Employee(DateTime onBoarding, string name, string surname)
        {
            if (onBoarding == DateTime.MinValue) throw new ArgumentOutOfRangeException(nameof(onBoarding));

            this.Id = new EmployeeId(Guid.NewGuid());
            this.OnboardingDate = onBoarding;
            this.Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : name;
            this.SurName = string.IsNullOrWhiteSpace(surname) ? throw new ArgumentNullException(nameof(surname)) : surname;
        }

        protected Employee() { }

        public static Employee Create(DateTime onBoarding, string name, string surname)
        {
            return new Employee(onBoarding, name, surname);
        }

        protected override void When(object @event)
        {
            //Do nothing as of now :)
        }

        public override void EnsureValidState()
        {
            //Do nothing as of now :)
        }
    }
}
