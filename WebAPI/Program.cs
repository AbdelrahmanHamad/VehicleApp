using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var nhtsaUrl = builder.Configuration.GetValue<string>("ExternalApis:NhtsaBaseUrl");
builder.Services.AddHttpClient<IVehicleService, VehicleService>(client =>
{
    client.BaseAddress = new Uri(nhtsaUrl ?? throw new InvalidOperationException("NHTSA Base URL is missing!"));
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
