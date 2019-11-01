using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using UOCHotels.RoomServiceManagement.Application.ReadModel;

namespace UOCHotels.RoomServiceManagement.Application.Hubs
{
    //TODO Configure [Authorize], parsing from the QueryString the Auth req
    //https://medium.com/@tarik.nzl/asp-net-core-and-signalr-authentication-with-the-javascript-client-d46c15584daf
    public class RoomServiceHub : Hub
    {
        public async Task GetRoomServiceUpdates(RoomServiceModel roomServiceModel)
        {
            await Clients.All.SendAsync(
                "ReceiveMessage",
                JsonConvert.SerializeObject(roomServiceModel));
        }
    }
}

