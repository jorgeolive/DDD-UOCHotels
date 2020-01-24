using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Application.ReadModel;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Domain.Events;

namespace UOCHotels.RoomServiceManagement.Application.Projections
{
    public class RoomServiceProjection : IProjection
    {
        private readonly IServiceScopeFactory scopeFactory;

        public RoomServiceProjection(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }
        public async Task Project(object @event)
        {
            switch (@event)
            {
                case ServiceCreated service:
                    {
                        using (var scope = scopeFactory.CreateScope())
                        {
                            var employeeRepository = scope.ServiceProvider.GetService<IEmployeeRepository>();
                            var roomRepository = scope.ServiceProvider.GetService<IRoomRepository>();
                            var roomServiceRepository = scope.ServiceProvider.GetService<IRoomServiceRepository>();

                            var roomModel = await roomRepository.GetById(service.RoomId);
                            var employee = await employeeRepository.GetById(service.EmployeeId);

                            await roomServiceRepository.Add(new RoomServiceModel()
                            {
                                Building = roomModel.Building,
                                Floor = roomModel.Floor,
                                RoomNumber = roomModel.Number,
                                Id = service.ServiceId,
                                RoomId = roomModel.Id,
                                Status = "Draft",
                                Owner = employee.Name + employee.SurName
                            });

                            await roomServiceRepository.Commit();
                        }

                        break;
                    }
                case ServicePlanned e:
                    {
                        using (var scope = scopeFactory.CreateScope())
                        {
                            var roomServiceRepository = scope.ServiceProvider.GetService<IRoomServiceRepository>();
                            var roomServiceModel = await roomServiceRepository.GetById(e.ServiceId);

                            roomServiceModel.Status = "Planned";
                            roomServiceModel.PlannedOn = e.PlannedOn;

                            await roomServiceRepository.Commit();
                        }

                        break;
                    }

                case ServiceFinished e:
                    {
                        using (var scope = scopeFactory.CreateScope())
                        {
                            var roomServiceRepository = scope.ServiceProvider.GetService<IRoomServiceRepository>();
                            var roomServiceModel = await roomServiceRepository.GetById(e.ServiceId);

                            roomServiceModel.Status = "Finished";
                            roomServiceModel.CompletedOn = e.Timestamp;

                            await roomServiceRepository.Commit();
                        }

                        break;
                    }

                case ServiceStarted e:
                    {
                        using (var scope = scopeFactory.CreateScope())
                        {
                            var roomServiceRepository = scope.ServiceProvider.GetService<IRoomServiceRepository>();
                            var roomServiceModel = await roomServiceRepository.GetById(e.ServiceId);

                            roomServiceModel.Status = "Started";
                            roomServiceModel.StartedOn = e.StartTimestamp;

                            await roomServiceRepository.Commit();
                        }

                        break;
                    }
            }
        }
    }
}
