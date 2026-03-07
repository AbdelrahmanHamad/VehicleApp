using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;
using FluentValidation;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IValidator<GetModelsRequest> _modelsValidator;
        private readonly IValidator<GetTypesRequest> _typesValidator;

        public VehicleController(
            IVehicleService vehicleService,
            IValidator<GetModelsRequest> modelsValidator,
            IValidator<GetTypesRequest> typesValidator)
        {
            _vehicleService = vehicleService;
            _modelsValidator = modelsValidator;
            _typesValidator = typesValidator;
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
            var request = new GetTypesRequest(makeId);
            var validationResult = await _typesValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

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
            var request = new GetModelsRequest(makeId, year);
            var validationResult = await _modelsValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var models = await _vehicleService.GetModelsAsync(makeId, year);
            return Ok(models);
        }
    }
}
