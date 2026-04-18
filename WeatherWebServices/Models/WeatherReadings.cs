namespace WeatherWebServices.Models
{
    public class WeatherReadings
    {

        public string StationId { get; set; }
        public string StationName { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public decimal Rainfall { get; set; }

        public string RainfallUnit { get; set; }

        public DateTimeOffset RainfallReadingTimeStamp { get; set; }

        public decimal Windspeed { get; set; }

        public string WindspeedUnit { get; set; }

        public DateTimeOffset WindspeedReadingTimeStamp { get; set; }
        public decimal WindDirection { get; set; }

        public string WindDirectionUnit { get; set; }

        public DateTimeOffset WindDirectionReadingTimeStamp { get; set; }
        public decimal Humidity { get; set; }

        public string HumidityUnit { get; set; }

        public DateTimeOffset HumidityReadingTimeStamp { get; set; }

        public decimal Temperature { get; set; }

        public string TemperatureUnit { get; set; }

        public DateTimeOffset TemperatureReadingTimeStamp { get; set; }
    }
}
