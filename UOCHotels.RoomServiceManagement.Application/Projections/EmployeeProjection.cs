using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Application.ReadModel;
using UOCHotels.RoomServiceManagement.Application.Repositories;
using UOCHotels.RoomServiceManagement.Domain.Events;

namespace UOCHotels.RoomServiceManagement.Application.Projections
{
    public class EmployeeProjection : IProjection
    {
        private readonly IServiceScopeFactory scopeFactory;
        public EmployeeProjection(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public async Task Project(object @event)
        {
            switch (@event)
            {
                case EmployeeCreated e:
                    {
                        using (var scope = scopeFactory.CreateScope())
                        {
                            var employeeRepository = scope.ServiceProvider.GetService<IEmployeeRepository>();

                            if ((await employeeRepository.GetById(e.EmployeeId)) != null)
                            {
                                return;
                                //throw new Exception("Employee already exists.");
                            }

                            await employeeRepository.Add(new EmployeeModel()
                            {
                                DateOfBirth = e.DateOfBirth,
                                Name = e.Name,
                                SurName = e.SurName,
                                SocialSecurityNumber = e.SocialSecurityNumber,
                                Id = e.EmployeeId
                            });

                            await employeeRepository.Commit();
                        }
                        break;
                    }
            }
        }
    }
}