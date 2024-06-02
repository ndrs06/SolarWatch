using SolarWatchAPI.Model.DataModels;
using SolarWatchAPI.Service.Repository;

namespace SolarWatchAPI.Service;

public class SunriseSunsetService : ISunriseSunsetService
{
    private readonly ILogger<SunriseSunsetService> _logger;
    private readonly ISunriseSunsetRepository _sunriseSunsetRepository;

    public SunriseSunsetService(ILogger<SunriseSunsetService> logger, ISunriseSunsetRepository sunriseSunsetRepository)
    {
        _logger = logger;
        _sunriseSunsetRepository = sunriseSunsetRepository;
    }

    public SunriseSunset? GetByCityIdAndDate(int cityId, DateTime date)
    {
        return _sunriseSunsetRepository.GetByCityAndDate(cityId, date);
    }

    public void Add(SunriseSunset sunriseSunset)
    {
        _sunriseSunsetRepository.Add(sunriseSunset);
    }
}