using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Application.ReadModel;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Domain.Events;

namespace UOCHotels.RoomServiceManagement.Application.Projections
{
    public class RoomIncidentProjection : IProjection
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<RoomIncidentProjection> logger;

        public RoomIncidentProjection(ILogger<RoomIncidentProjection> logger, IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
            this.logger = logger;
        }
        public async Task Project(object @event)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var roomIncidentRepository = scope.ServiceProvider.GetService<IRoomIncidentRepository>();
                var roomRepository = scope.ServiceProvider.GetService<IRoomRepository>();
                var employeeRepository = scope.ServiceProvider.GetService<IEmployeeRepository>();

                switch (@event)
                {
                    case RoomIncidentCreated e:
                        {
                            logger.LogInformation($"Event to project: {JsonConvert.SerializeObject(e)}");

                            var room = await roomRepository.GetById(e.RoomId);
                            var employee = await employeeRepository.GetById(e.EmployeeId);

                            await roomIncidentRepository.Add(new RoomIncidentModel()
                            {
                                Id = e.RoomIncidentId,
                                Building = room.Building,
                                EmployeeId = e.EmployeeId,
                                RoomFloor = room.Floor,
                                RoomNumber = room.Number,
                                CreatedByFullName = employee.Name + " " + employee.SurName,
                                CreatedOn = e.CreatedOn,
                                Comment = e.Comment,
                                RoomId = e.RoomId
                            });

                            await roomIncidentRepository.Commit();
                        }
                        break;

                    case PictureAdded e:
                        {
                            var roomIncident = await roomIncidentRepository.GetById(e.RoomIncidentId);

                            roomIncident.Pictures.Add(e.Uri);

                            await roomIncidentRepository.Commit();
                        }
                        break;
                }
            }
        }
    }
}
