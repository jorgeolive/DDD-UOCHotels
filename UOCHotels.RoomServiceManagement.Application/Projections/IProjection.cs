using System.Threading.Tasks;

namespace UOCHotels.RoomServiceManagement.Application.Projections
{
    public interface IProjection
    {
        Task Project(object @event);
    }
}
