using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("makes")]
        public async Task<ActionResult<List<VehicleMakeDto>>> GetAllMakes()
        {
            var makes = await _vehicleService.GetAllMakesAsync();
            return Ok(makes);
        }

        [HttpGet("types/{makeId}")]
        public async Task<ActionResult<List<VehicleTypeDto>>> GetTypes(int makeId)
        {
            var types = await _vehicleService.GetVehicleTypesAsync(makeId);
            if (types == null || !types.Any())
            {
                return NotFound($"No vehicle types found for Make ID {makeId}");
            }
            return Ok(types);
        }

        [HttpGet("models/{makeId}/{year}")]
        public async Task<ActionResult<List<VehicleModelDto>>> GetModels(int makeId, int year)
        {
            var models = await _vehicleService.GetModelsAsync(makeId, year);
            return Ok(models);
        }
    }
}
