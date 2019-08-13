using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Exceptions;
using UOCHotels.RoomServiceManagement.Application.Infraestructure;
using UOCHotels.RoomServiceManagement.Domain;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Handlers
{
    public class StartRoomServiceCommandHandler : AsyncRequestHandler<StartRoomServiceCommand>
    {
        private readonly IRoomServiceManagementContext dbContext;
        private readonly IMediator mediator;

        public StartRoomServiceCommandHandler(IRoomServiceManagementContext context, IMediator mediator)
        {
            dbContext = context;
            this.mediator = mediator;
        }

        protected override async Task Handle(StartRoomServiceCommand request, CancellationToken cancellationToken)
        {
            RoomService room = null;

            try
            {
                room = await this.dbContext.RoomServiceContext.FindAsync(new RoomServiceId(request.RoomServiceId));
            }
            catch
            {
                throw new RoomServiceNotFoundException();
            }

            room.Start();
            //Here we might want to notify other services through the messaging queue. 
        }
    }
}
