using MediatR;
using System;

namespace UOCHotels.RoomServiceManagement.Domain.Events
{
    public class EmployeeCreated : INotification
    {
        public EmployeeCreated(Guid employeeId, string name, string surName, DateTime onboardingDate, string socialSecurityNumber, DateTime dateOfBirth)
        {
            EmployeeId = employeeId;
            Name = name;
            SurName = surName;
            OnboardingDate = onboardingDate;
            SocialSecurityNumber = socialSecurityNumber;
            DateOfBirth = dateOfBirth;
        }

        public Guid EmployeeId { get; internal set; }
        public string Name { get; internal set; }
        public string SurName { get; internal set; }
        public DateTime OnboardingDate { get; internal set; }
        public string SocialSecurityNumber { get; internal set; }
        public DateTime DateOfBirth { get; internal set; }
    }
}
