using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Globalization;
using WeatherWebServices.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static WeatherWebServices.Models.WeatherForecastResponseApi;

namespace WeatherWebServices.Data
{
    public class WeatherForecastService
    {
        private readonly HttpClient _httpClient;
        private readonly IWeatherForecastRepository _repository;
        private readonly ILogger<WeatherForecastService> _logger;
        private readonly WeatherSettings _settings;
 
        public WeatherForecastService(HttpClient httpClient, IWeatherForecastRepository repository, ILogger<WeatherForecastService> logger, IOptions<WeatherSettings> options)
        {
            _httpClient = httpClient;
            _repository = repository;
            _logger = logger;
            _settings = options.Value;
        }


        // Sync Weatherforecast
        public async Task<WeatherForecastResponseApi> SyncWeatherDataAsync(string? Forecastdate="")
        {


            string format = "yyyy-MM-dd";

            bool isValid = DateTime.TryParseExact(
                Forecastdate,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime validatedDate);
            var requestUri = $"{_settings.BaseUrl}/twenty-four-hr-forecast";
            if ((Forecastdate != null) && (Forecastdate != string.Empty)) // checking date format
            {

                    if (!isValid)
                    {

                        throw new FormatException($"Invalid date: '{Forecastdate}'. Please use the format {format}.");
                        return null;
                    }


           
                requestUri = requestUri + "?date=" + Forecastdate;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);


            request.Headers.Add("X-API-KEY", _settings.ApiKey);
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var weatherData = await response.Content.ReadFromJsonAsync<WeatherForecastResponseApi>();
            
            await _repository.ProcessForecastAsync(weatherData);

            return weatherData;

        }


        // Sync & trigger alerts
        public async Task SyncAndTriggerAlert (string? Forecastdate = "")
        {
            try
            {
                await SyncWeatherDataAsync();
                await _repository.ExecuteSubscriptionAlert(Convert.ToDateTime(Forecastdate));
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}