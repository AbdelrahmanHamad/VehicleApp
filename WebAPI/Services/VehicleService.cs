using Microsoft.Extensions.Caching.Memory;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class VehicleService : IVehicleService
    {

        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private const string MakesCacheKey = "VehicleMakes";

        public VehicleService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<List<VehicleMakeDto>> GetAllMakesAsync()
        {
            if (_cache.TryGetValue(MakesCacheKey, out List<VehicleMakeDto>? makes))
            {
                return makes!;
            }

            var response = await _httpClient.GetFromJsonAsync<NhtsaResponse<VehicleMakeDto>>("getallmakes?format=json");
            makes = response?.Results ?? new List<VehicleMakeDto>();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

            _cache.Set(MakesCacheKey, makes, cacheOptions);

            return makes;
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
