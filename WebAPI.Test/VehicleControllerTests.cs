using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Models;
using WebAPI.Services;
using Xunit;

namespace WebAPI.Test
{
    public class VehicleControllerTests
    {
        private readonly Mock<IVehicleService> _mockService;
        private readonly Mock<IValidator<GetModelsRequest>> _mockModelsValidator;
        private readonly Mock<IValidator<GetTypesRequest>> _mockTypesValidator;
        private readonly VehicleController _controller;

        public VehicleControllerTests()
        {
            _mockService = new Mock<IVehicleService>();
            _mockModelsValidator = new Mock<IValidator<GetModelsRequest>>();
            _mockTypesValidator = new Mock<IValidator<GetTypesRequest>>();
            _controller = new VehicleController(_mockService.Object, _mockModelsValidator.Object, _mockTypesValidator.Object);
        }

        [Fact]
        public async Task GetAllMakes_ReturnsOk()
        {
            var makes = new List<VehicleMakeDto> { new() { Make_ID = 1, Make_Name = "Test" } };
            _mockService.Setup(s => s.GetAllMakesAsync()).ReturnsAsync(makes);

            var result = await _controller.GetAllMakes();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(makes);
        }

        [Fact]
        public async Task GetTypes_ReturnsOk_WhenValid()
        {
            int makeId = 1;
            var types = new List<VehicleTypeDto> { new() { VehicleTypeId = 1, VehicleTypeName = "Car" } };
            _mockTypesValidator.Setup(v => v.ValidateAsync(It.IsAny<GetTypesRequest>(), default))
                .ReturnsAsync(new ValidationResult());
            _mockService.Setup(s => s.GetVehicleTypesAsync(makeId)).ReturnsAsync(types);

            var result = await _controller.GetTypes(makeId);

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(types);
        }

        [Fact]
        public async Task GetModels_ReturnsOk_WhenValid()
        {
            int makeId = 1, year = 2020;
            var models = new List<VehicleModelDto> { new() { Model_ID = 1, Model_Name = "Model X" } };
            _mockModelsValidator.Setup(v => v.ValidateAsync(It.IsAny<GetModelsRequest>(), default))
                .ReturnsAsync(new ValidationResult());
            _mockService.Setup(s => s.GetModelsAsync(makeId, year)).ReturnsAsync(models);

            var result = await _controller.GetModels(makeId, year);

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(models);
        }

        [Fact]
        public async Task GetModels_ReturnsBadRequest_WhenInvalid()
        {
            int makeId = 1, year = 1880;
            var validationFailures = new List<ValidationFailure> { new("Year", "Invalid year") };
            _mockModelsValidator.Setup(v => v.ValidateAsync(It.IsAny<GetModelsRequest>(), default))
                .ReturnsAsync(new ValidationResult(validationFailures));

            var result = await _controller.GetModels(makeId, year);

            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(400);
        }
    }
}
