using System;
using MediatR;

namespace UOCHotels.RoomServiceManagement.Application.IntegrationEvents
{
    public class RoomOccupationStarted : INotification
    {
        public Guid RoomId { get; private set; }
        public int RoomNumber { get; private set; }
        public int Floor { get; private set; }
        public string Building { get; private set; }
        public DateTime OccupationEndDate { get; private set; }
        public string CustomerName { get; private set; }
        public string CustomerSurname { get; private set; }

        public RoomOccupationStarted(
                      int roomNumber,
                      int floor,
                      string building,
                      string customerName,
                      string customerSurname,
                      DateTime occupationEndDate)
        {
            RoomNumber = roomNumber;
            Floor = floor;
            Building = building;
            CustomerName = customerName;
            CustomerSurname = customerSurname;
            OccupationEndDate = occupationEndDate;
        }
    }
}
