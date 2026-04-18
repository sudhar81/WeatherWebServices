namespace WeatherWebServices.Models
{
    public class WeatherForecast
    {

        public DateTime ForecastDate { get; set; }
        public string TempHigh { get; set; }

        public string TempLow { get; set; }

        public string TempUnits { get; set; }

        public int HumidityHigh { get; set; }

        public int HumidityLow{get; set; }

        public string HumidityUnits { get; set; }

        public string Forecastcode { get; set; }
        public string ForecastText { get; set; }

        public DateTime ValidPeriodStart { get; set; }
        public DateTime ValidPeriodEnd { get; set; }

        public string? ValidPeriodText { get; set; }
        public int WindSpeedHigh { get; set; }

        public int WindSpeedLow { get; set; }

        public string? WindDirection { get; set; }

        public DateTimeOffset UpdatedTimestamp { get; set; }

        public List<RegionalForecast> regionalForecasts { get; set; }

    }


    public class RegionalForecast
    {

      
        public string? RegionName { get; set; }

        public string? ForecastCode { get; set; }

        public string? ForecastText { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public string? TimePeriodText { get; set; }

       
    }


    public class WeatherForecastExport
    {

        public DateTime ForecastDate { get; set; }
        public string TempHigh { get; set; }

        public string TempLow { get; set; }

        public string TempUnits { get; set; }

        public int HumidityHigh { get; set; }

        public int HumidityLow { get; set; }

        public string HumidityUnits { get; set; }

        public string Forecastcode { get; set; }
        public string ForecastText { get; set; }

        public DateTime ValidPeriodStart { get; set; }
        public DateTime ValidPeriodEnd { get; set; }

        public string? ValidPeriodText { get; set; }
        public int WindSpeedHigh { get; set; }

        public int WindSpeedLow { get; set; }

        public string? WindDirection { get; set; }

        public DateTimeOffset UpdatedTimestamp { get; set; }

   

        public string? RegionName { get; set; }

        public string? RegionalForecastCode { get; set; }

        public string? RegionalForecastText { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public string? TimePeriodText { get; set; }


    }


}
