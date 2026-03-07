using WebAPI.Models;

namespace WebAPI.Services
{
    public class VehicleService : IVehicleService
    {

        private readonly HttpClient _httpClient;

        public VehicleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<VehicleMakeDto>> GetAllMakesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<NhtsaResponse<VehicleMakeDto>>("getallmakes?format=json");
            return response?.Results ?? new List<VehicleMakeDto>();
        }

        public async Task<List<VehicleModelDto>> GetModelsAsync(int makeId, int year)
        {
            var response = await _httpClient.GetFromJsonAsync<NhtsaResponse<VehicleModelDto>>($"GetModelsForMakeIdYear/makeId/{makeId}/modelyear/{year}?format=json");
            return response?.Results ?? new List<VehicleModelDto>();
        }

        public async Task<List<VehicleTypeDto>> GetVehicleTypesAsync(int makeId)
        {
            var response = await _httpClient.GetFromJsonAsync<NhtsaResponse<VehicleTypeDto>>($"GetVehicleTypesForMakeId/{makeId}?format=json");
            return response?.Results ?? new List<VehicleTypeDto>();
        }
    }
}
