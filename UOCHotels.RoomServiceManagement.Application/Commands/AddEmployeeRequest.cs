using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Application.Commands
{
    public class AddEmployeeRequest : IRequest
    {
        public DateTime OnBoardingDate;
        public string Name;
        public string SurName;

        private AddEmployeeRequest() { }

        public AddEmployeeRequest(string name, string surName, DateTime onBoarding)
        {
            this.Name = name;
            this.SurName = surName;
            this.OnBoardingDate = onBoarding;
        }
    }
}
