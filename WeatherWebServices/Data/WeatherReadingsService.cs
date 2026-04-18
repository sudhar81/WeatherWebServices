 
using Microsoft.Extensions.Options;
using System.Globalization;
using WeatherWebServices.Models;
 
namespace WeatherWebServices.Data
{
    public class WeatherReadingsService
    {
        private readonly HttpClient _httpClient;
        private readonly WeatherReadingsRepository _repository;
        private readonly ILogger<WeatherReadingsService> _logger;
        private readonly WeatherSettings _settings;

        public WeatherReadingsService(HttpClient httpClient, WeatherReadingsRepository repository, ILogger<WeatherReadingsService> logger, IOptions<WeatherSettings> options)
        {
            _httpClient = httpClient;
            _repository = repository;
            _logger = logger;
            _settings = options.Value;
        }

       

        

        // Fetch LatestValues storein temp table reply back 

        public async Task FetchLatestWeatherReadings(string SessionId,string Date)
        {
            if ((Date != null) && (Date != string.Empty))
            {

                string format = "yyyy-MM-dd";

                bool isValid = DateTime.TryParseExact(
                    Date,
                    format,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime validatedDate);

                if (!isValid)
                {

                    throw new FormatException($"Invalid date: '{Date}'. Please use the format {format}.");
                    return;
                }
            }


            await FetchAndStoreTemperatureAsync(SessionId, Date);
            await FetchAndStoreRainfallAsync(SessionId, Date);

            await FetchAndStoreHumidityAsync(SessionId, Date);

            await FetchAndStoreWindDirectionAsync(SessionId, Date);
            await FetchAndStoreWindSpeedAsync(SessionId, Date);
        }


        // Temperature Temp
        public async Task FetchAndStoreTemperatureAsync(string SessionId, string WeatherDate)
        {
            var requestUri = $"{_settings.BaseUrl}/air-temperature";
            if ((WeatherDate != null) && (WeatherDate !=string.Empty))
            {
                requestUri= requestUri +"?date="+ WeatherDate  ;
            }
            

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);


            request.Headers.Add("X-API-KEY", _settings.ApiKey);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<WeatherReadingResponseApi>();

            if (json?.Data?.ReadingsValues == null || !json.Data.ReadingsValues.Any())
            {
                return;
            }

            //  Prepare Transactional Data (Readings)
            var latestContainer = json.Data.ReadingsValues.First();
            var ReadingType = json.Data.ReadingType;
            var ReadingUnit = json.Data.ReadingUnit;

            // Create the specific list type the repository expects
            var temperatureReadings = latestContainer.StationValues.Select(r => new ReponseData
            {
                StationId = r.StationId,
                Value = r.Value,
                ReadingType = ReadingType,
                ReadingUnit = ReadingUnit,
                ReadingTimestamp = latestContainer.Timestamp,
                SessionId = SessionId
            }).ToList();

            //   Prepare Master Data
            var stationMaster = json.Data.WeatherStations;

            //  Pass to Repository 
            await _repository.ProcessTemperatureAsync(stationMaster, temperatureReadings);
        }


        // Rain fall
        public async Task FetchAndStoreRainfallAsync(string SessionId, string WeatherDate)
        {
        
        


            var requestUri = $"{_settings.BaseUrl}/rainfall";
            if ((WeatherDate != null) && (WeatherDate != string.Empty))
            {
                requestUri = requestUri + "?date=" + WeatherDate;
            }


            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);


            request.Headers.Add("X-API-KEY", _settings.ApiKey);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<WeatherReadingResponseApi>();

            if (json?.Data?.ReadingsValues == null || !json.Data.ReadingsValues.Any())
            {
                return;
            }

            //  Prepare Transactional Data (Readings)
            var latestContainer = json.Data.ReadingsValues.First();
            var ReadingType = json.Data.ReadingType;
            var ReadingUnit = json.Data.ReadingUnit;

            // Create the specific list type the repository expects
            var rainfallRecords = latestContainer.StationValues.Select(r => new ReponseData
            {
                StationId = r.StationId,
                Value = r.Value,
                ReadingType = ReadingType,
                ReadingUnit = ReadingUnit,
                ReadingTimestamp = latestContainer.Timestamp,
                 SessionId = SessionId
            }).ToList();

            //   Prepare Master Data
            var stationMaster = json.Data.WeatherStations;

             
            await _repository.ProcessRainfallAsync(stationMaster, rainfallRecords);
        }


        // Humidity Reading
        public async Task FetchAndStoreHumidityAsync(string SessionId, string WeatherDate)
        {
             var requestUri = $"{_settings.BaseUrl}/relative-humidity";
            if ((WeatherDate != null) && (WeatherDate != string.Empty))
            {
                requestUri = requestUri + "?date=" + WeatherDate;
            }


            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);


            request.Headers.Add("X-API-KEY", _settings.ApiKey);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<WeatherReadingResponseApi>();

            if (json?.Data?.ReadingsValues == null || !json.Data.ReadingsValues.Any())
            {
                return;
            }

            //  Prepare Transactional Data (Readings)
            var latestContainer = json.Data.ReadingsValues.First();
            var ReadingType = json.Data.ReadingType;
            var ReadingUnit = json.Data.ReadingUnit;

            // Create the specific list type the repository expects
            var temperatureReadings = latestContainer.StationValues.Select(r => new ReponseData
            {
                StationId = r.StationId,
                Value = r.Value,
                ReadingType = ReadingType,
                ReadingUnit = ReadingUnit,
                ReadingTimestamp = latestContainer.Timestamp,
                 SessionId = SessionId
            }).ToList();

            //   Prepare Master Data
            var stationMaster = json.Data.WeatherStations;

            
            await _repository.ProcessHumidityAsync(stationMaster, temperatureReadings);
        }


        //WindDirection
        public async Task FetchAndStoreWindDirectionAsync(string SessionId, string WeatherDate)
        {
             

            var requestUri = $"{_settings.BaseUrl}/wind-direction";
            if ((WeatherDate != null) && (WeatherDate != string.Empty))
            {
                requestUri = requestUri + "?date=" + WeatherDate;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);


            request.Headers.Add("X-API-KEY", _settings.ApiKey);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<WeatherReadingResponseApi>();

            if (json?.Data?.ReadingsValues == null || !json.Data.ReadingsValues.Any())
            {
                return;
            }

            //  Prepare Transactional Data (Readings)
            var latestContainer = json.Data.ReadingsValues.First();
            var ReadingType = json.Data.ReadingType;
            var ReadingUnit = json.Data.ReadingUnit;

            // Create the specific list type the repository expects
            var Readings = latestContainer.StationValues.Select(r => new ReponseData
            {
                StationId = r.StationId,
                Value = r.Value,
                ReadingType = ReadingType,
                ReadingUnit = ReadingUnit,
                ReadingTimestamp = latestContainer.Timestamp,
                 SessionId = SessionId
            }).ToList();

            //   Prepare Master Data
            var stationMaster = json.Data.WeatherStations;

            
            await _repository.ProcessWindDirectionAsync(stationMaster, Readings);
        }



        // WindSpeed 
        public async Task FetchAndStoreWindSpeedAsync(string SessionId, string WeatherDate)
        {
 
            var requestUri = $"{_settings.BaseUrl}/wind-speed";
            if ((WeatherDate != null) && (WeatherDate != string.Empty))
            {
                requestUri = requestUri + "?date=" + WeatherDate;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);


            request.Headers.Add("X-API-KEY", _settings.ApiKey);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<WeatherReadingResponseApi>();

            if (json?.Data?.ReadingsValues == null || !json.Data.ReadingsValues.Any())
            {
                return;
            }

            //  Prepare Transactional Data (Readings)
            var latestContainer = json.Data.ReadingsValues.First();
            var ReadingType = json.Data.ReadingType;
            var ReadingUnit = json.Data.ReadingUnit;

            // Create the specific list type the repository expects
            var Readings = latestContainer.StationValues.Select(r => new ReponseData
            {
                StationId = r.StationId,
                Value = r.Value,
                ReadingType = ReadingType,
                ReadingUnit = ReadingUnit,
                ReadingTimestamp = latestContainer.Timestamp,
                 SessionId = SessionId
            }).ToList();

            //   Prepare Master Data
            var stationMaster = json.Data.WeatherStations;

            
            await _repository.ProcessWindSpeedAsync(stationMaster, Readings);
        }

    }
}
