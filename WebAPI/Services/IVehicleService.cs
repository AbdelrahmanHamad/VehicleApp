using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IVehicleService
    {
        public Task<List<VehicleMakeDto>> GetAllMakesAsync();
        public Task<List<VehicleTypeDto>> GetVehicleTypesAsync(int makeId);
        public Task<List<VehicleModelDto>> GetModelsAsync(int makeId, int year);
    }
}
