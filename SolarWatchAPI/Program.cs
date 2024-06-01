using SolarWatchAPI.Data;
using SolarWatchAPI.Service.DataProviders;
using SolarWatchAPI.Service.JsonProcessors;
using SolarWatchAPI.Service.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SolarWatchApiContext>();

builder.Services.AddSingleton<ICoordinatesProvider, CoordinatesProvider>();
builder.Services.AddSingleton<ISolarWatchDataProvider, SolarWatchDataProvider>();
builder.Services.AddSingleton<IJsonProcessor, JsonProcessor>();

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ISunriseSunsetRepository, SunsetSunriseRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();