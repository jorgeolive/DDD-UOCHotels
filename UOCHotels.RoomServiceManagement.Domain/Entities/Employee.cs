using System;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;

namespace UOCHotels.RoomServiceManagement.Domain
{
    public class Employee : Entity
    {
        public string Name { get; internal set; }
        public string SurName { get; internal set; }
        public DateTime OnboardingDate { get; internal set; }
        public int ExperienceMonths => Math.Abs((DateTime.UtcNow.Month - OnboardingDate.Date.Month) + 12 * (DateTime.UtcNow.Year - OnboardingDate.Date.Year));

        public Employee(DateTime onBoarding, string name, string surname)
        {
            if (onBoarding == DateTime.MinValue) throw new ArgumentOutOfRangeException(nameof(onBoarding));

            this.OnboardingDate = onBoarding;
            this.Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : name;
            this.SurName = string.IsNullOrWhiteSpace(surname) ? throw new ArgumentNullException(nameof(surname)) : surname;
        }

        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }

        public override void EnsureValidState()
        {
            throw new NotImplementedException();
        }
    }
}
