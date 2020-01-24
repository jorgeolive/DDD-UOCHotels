using System;
using System.IO;
using System.Threading.Tasks;

namespace UOCHotels.RoomServiceManagement.Application.Services
{
    public interface IFileStorageService
    {
        Task<Uri> SaveFile(string fileName, Stream file);
    }
}
