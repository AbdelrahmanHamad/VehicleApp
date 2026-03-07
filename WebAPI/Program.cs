using WebAPI.Middleware;
using WebAPI.Services;
using WebAPI.Validators;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var nhtsaUrl = builder.Configuration.GetValue<string>("ExternalApis:NhtsaBaseUrl");
builder.Services.AddHttpClient<IVehicleService, VehicleService>(client =>
{
    client.BaseAddress = new Uri(nhtsaUrl ?? throw new InvalidOperationException("NHTSA Base URL is missing!"));
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});


builder.Services.AddControllers();


builder.Services.AddValidatorsFromAssemblyContaining<GetModelsRequestValidator>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseExceptionHandler();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
