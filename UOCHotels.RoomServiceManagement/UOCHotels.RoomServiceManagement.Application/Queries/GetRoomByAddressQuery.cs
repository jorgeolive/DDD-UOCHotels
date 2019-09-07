using MediatR;
using UOCHotels.RoomServiceManagement.Application.ReadModel;

namespace UOCHotels.RoomServiceManagement.Application.Queries
{
    public class GetRoomByAddressQuery : IRequest<RoomModel>
    {
        public string BuildingName;
        public int RoomNumber;
        public int Floor;
    }
}

