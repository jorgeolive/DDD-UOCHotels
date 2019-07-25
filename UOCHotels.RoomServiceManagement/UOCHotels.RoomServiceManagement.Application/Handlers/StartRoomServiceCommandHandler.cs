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

        public StartRoomServiceCommandHandler(IRoomServiceManagementContext context)
        {
            dbContext = context;
        }

        protected override async Task Handle(StartRoomServiceCommand request, CancellationToken cancellationToken)
        {
            var roomService = this.dbContext.RoomServiceContext.Any(x => x.Id == request.RoomServiceId) ? this.dbContext.RoomServiceContext.Where(x => x.Id == request.RoomServiceId).Single() : throw new RoomServiceNotFoundException(); 

            roomService.Start();

            await this.dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
