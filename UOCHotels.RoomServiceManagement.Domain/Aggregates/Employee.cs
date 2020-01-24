using System;
using UOCHotels.RoomServiceManagement.Domain.Events;
using UOCHotels.RoomServiceManagement.Domain.SeedWork;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Domain.Aggregates
{
    public class Employee : AggregateRoot<EmployeeId>
    {
        public bool IsActive { get; internal set; }
        public string Name { get; internal set; }
        public string SurName { get; internal set; }
        public DateTime OnboardingDate { get; internal set; }
        public bool OnLeave { get; internal set; } = false;
        public int ExperienceMonths => Math.Abs((DateTime.UtcNow.Month - OnboardingDate.Date.Month) + 12 * (DateTime.UtcNow.Year - OnboardingDate.Date.Year));
        public string SocialSecurityNumber { get; internal set; }
        public DateTime DateOfBirth { get; internal set; }
        public DateTime? ExpectedReturn { get; internal set; }

        //To do : Add vacations

        private Employee(string name, string surName, DateTime onboardingDate, string socialSecurityNumber, DateTime dateOfBirth)
        {
            Apply(new EmployeeCreated(Guid.NewGuid(), name, surName, onboardingDate, socialSecurityNumber, dateOfBirth));
        }

        protected Employee() { }

        public static Employee Create(string name, string surName, DateTime onboardingDate, string socialSecurityNumber, DateTime dateOfBirth)
        {
            if (onboardingDate == DateTime.MinValue) throw new ArgumentOutOfRangeException(nameof(onboardingDate));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(surName)) throw new ArgumentNullException(nameof(surName));
            if (string.IsNullOrWhiteSpace(socialSecurityNumber)) throw new ArgumentNullException(nameof(socialSecurityNumber));
            if (dateOfBirth == DateTime.MinValue) throw new ArgumentOutOfRangeException(nameof(dateOfBirth));

            return new Employee(name, surName, onboardingDate, socialSecurityNumber, dateOfBirth);
        }

        public void SetOnLeave(DateTime? returnDate)
        {
            if (OnLeave)
                throw new InvalidOperationException("Employee is already on leave.");
            Apply(new EmployeeOnLeaveStarted(this.Id.Value, returnDate));
        }

        public void EndOnLeave(DateTime? returnDate)
        {
            if (!OnLeave)
                throw new InvalidOperationException("Employee is not on leave.");
            Apply(new EmployeeOnLeaveStarted(this.Id.Value, returnDate));
        }

        public override void EnsureValidState()
        {           
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case EmployeeCreated e:
                    {
                        this.Id = EmployeeId.CreateFor(e.EmployeeId);
                        this.Name = e.Name;
                        this.SurName = e.SurName;
                        this.SocialSecurityNumber = e.SocialSecurityNumber;
                        this.DateOfBirth = e.DateOfBirth;
                        this.OnboardingDate = e.OnboardingDate;
                    }
                    break;

                case EmployeeOnLeaveStarted e:
                    {
                        this.OnLeave = true;
                        this.ExpectedReturn = e.ReturnDate.Value;
                    }
                    break;

                case EmployeeOnLeaveFinished e:
                    {
                        this.OnLeave = false;
                        this.ExpectedReturn = null;
                    }
                    break;
            }
        }
    }
}
