using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Application.Services;
using UOCHotels.RoomServiceManagement.Domain.Aggregates;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Handlers.Commands
{
    public class AddRoomIncidentPictureHandler : AsyncRequestHandler<AddRoomIncidentPictureRequest>
    {
        private readonly IFileStorageService _fileService;
        private readonly IAggregateStore _store;
        public AddRoomIncidentPictureHandler(
            IAggregateStore store, 
            IFileStorageService fileService)
        {
            _fileService = fileService;
            _store = store;
        }

        protected override async Task Handle(AddRoomIncidentPictureRequest request, CancellationToken cancellationToken)
        {
            if (!((await _store.Load<RoomIncident, RoomIncidentId>(RoomIncidentId.CreateFor(request.RoomIncidentId))) is RoomIncident roomIncident))
                throw new ArgumentNullException("RoomIncident not exists");

            var uri = await _fileService.SaveFile(request.File.FileName, request.File.OpenReadStream()) ;

            roomIncident.AddPicture(uri.ToString());

            await _store.Save<RoomIncident, RoomIncidentId>(roomIncident);
        }
    }
}
