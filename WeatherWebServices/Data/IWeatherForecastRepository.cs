using WeatherWebServices.Models;

namespace WeatherWebServices.Data
{
    public interface IWeatherForecastRepository
    {

       
         Task ProcessForecastAsync(WeatherForecastResponseApi weatherData);
        Task<WeatherForecast?> GetWeatherForecast(DateTime Fetchdate, string? RegionName);
        Task<IEnumerable<WeatherForecastExport?>> GetWeatherForecastExport(DateTime Fetchdate, string? RegionName);
        Task AlertsSubscribe(Subscribe subscribe);
        Task AlertsUnSubscribe(string email);
        Task ExecuteSubscriptionAlert(DateTime Fetchdate);
    }

}
