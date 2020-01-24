using System;
namespace UOCHotels.RoomServiceManagement.Application.ReadModel
{
    public class EmployeeModel : IReadModel
    {
        public Guid Id;
        public DateTime OnBoardDate;
        public string Name;
        public string SurName;
        public string SocialSecurityNumber;
        public DateTime DateOfBirth;

        public string DbId => Id.ToString();

        public string GetId()
        {
            return Id.ToString();
        }
    }
}
