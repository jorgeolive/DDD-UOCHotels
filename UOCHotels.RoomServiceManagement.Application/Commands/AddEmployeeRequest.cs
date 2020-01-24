using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Application.Commands
{
    public class AddEmployeeRequest : IRequest
    {
        public DateTime OnBoardingDate;
        public string Name;
        public string SurName;
        public string SocialSecurityNumber;
        public DateTime DateOfBirth;

        private AddEmployeeRequest() { }

        public AddEmployeeRequest(string name, string surName, DateTime onBoarding, string socialSecurityNumber, DateTime dateOfBirth)
        {
            Name = name;
            SurName = surName;
            OnBoardingDate = onBoarding;
            SocialSecurityNumber = socialSecurityNumber;
            DateOfBirth = dateOfBirth;
        }
    }
}
