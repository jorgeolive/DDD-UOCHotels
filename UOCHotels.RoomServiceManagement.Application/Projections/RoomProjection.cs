using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Application.ReadModel;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Domain.Events;

namespace UOCHotels.RoomServiceManagement.Application.Projections
{
    public class RoomProjection : IProjection
    {
        private readonly IServiceScopeFactory scopeFactory;

        public RoomProjection(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }
        public async Task Project(object @event)
        {
            using(var scope = scopeFactory.CreateScope())
            {
                var roomRepository = scope.ServiceProvider.GetService<IRoomRepository>();

                switch (@event)
                {          
                    case RoomCreated e:
                        {
                            await roomRepository.Add(new RoomModel()
                            {
                                Building = e.Building,
                                Floor = e.Floor,
                                Number = e.DoorNumber,
                                Id = e.RoomId
                            });

                            await roomRepository.Commit();

                            break;
                        }
                    case RoomOccupationDateChanged e:
                        {
                            var room = await roomRepository.GetById(e.RoomId);

                            room.OccupationEndDate = e.OccupationEndDate;

                            await roomRepository.Commit();
                        }
                        break;

                    case RoomOccupied e:
                        {
                            var room = await roomRepository.GetById(e.RoomId);

                            room.Occupied = true;
                            room.OccupationStartDate = e.OccupatiedOn;

                            await roomRepository.Commit();
                        }
                        break;
                }
            }
        }
    }
}
