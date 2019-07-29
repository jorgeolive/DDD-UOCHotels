using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UOCHotels.RoomServiceManagement.Application.Commands;
using UOCHotels.RoomServiceManagement.Application.Exceptions;
using UOCHotels.RoomServiceManagement.Application.Infraestructure;

namespace UOCHotels.RoomServiceManagement.Application.Handlers
{
    public class StartRoomServiceCommandHandler : AsyncRequestHandler<StartRoomServiceCommand>
    {
        private readonly IRoomServiceManagementContext dbContext;
        private readonly IMediator mediator;


        public StartRoomServiceCommandHandler(IRoomServiceManagementContext context, IMediator mediator)
        {
            dbContext = context;
        }

        protected override async Task Handle(StartRoomServiceCommand request, CancellationToken cancellationToken)
        {
            var room = this.dbContext.RoomContext.Any(x =>
                                                        x.Id == request.RoomId &&
                                                        x.RoomServices.Any(rs => rs.Id == request.RoomServiceId))
                ? this.dbContext.RoomContext.Where(x =>
                                                        x.Id == request.RoomId &&
                                                        x.RoomServices.Any(rs => rs.Id == request.RoomServiceId))
                                            .Single()
                : throw new RoomServiceNotFoundException();

            room.StartService(request.RoomServiceId);

            await this.dbContext.SaveChangesAsync(cancellationToken);
            await mediator.Publish(room.);
            //raise events. 
        }
    }
}
