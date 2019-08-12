using System;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Application.Infraestructure;
using UOCHotels.RoomServiceManagement.Domain.ValueObjects;

namespace UOCHotels.RoomServiceManagement.Application.Services
{
    public class EstimateRoomServiceCalculatorCommandHandler
    {
        private readonly IRoomServiceManagementContext _context;

        public EstimateRoomServiceCalculator(IRoomServiceManagementContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> Handle(RoomId roomId, EmployeeId employeeId)
        {
            var room = await _context.RoomContext.FindAsync();
            var employee = await _context.EmployeeContext.FindAsync(roomId);


        }
    }
}
