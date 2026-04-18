

using System.Text.Json.Serialization;


namespace WeatherWebServices.Models
{


    public class WeatherForecastResponseApi
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("data")]
        public WeatherData Data { get; set; }

        [JsonPropertyName("errorMsg")]
        public string ErrorMsg { get; set; }
    }


    public class WeatherData
    {
        [JsonPropertyName("records")]
        public List<WeatherRecord>  Records { get; set; }
    }
 

        public class WeatherRecord { 
            [JsonPropertyName("date")]            public string Date { get; set; }

            [JsonPropertyName("updatedTimestamp")]            public DateTime UpdatedTimestamp { get; set; }
            [JsonPropertyName("general")] public GeneralForecast General { get; set; }
            [JsonPropertyName("periods")] public List<ForecastPeriod> Periods { get; set; }
            [JsonPropertyName("timestamp")] public DateTime Timestamp { get; set; }
        }

        public class GeneralForecast { 
                    [JsonPropertyName("temperature")] public RangeValue Temperature { get; set; }
            [JsonPropertyName("relativeHumidity")] public RangeValue Humidity { get; set; }
            [JsonPropertyName("forecast")] public ForecastText Forecast { get; set; }
            [JsonPropertyName("validPeriod")] public ValidPeriod ValidPeriod { get; set; }
            [JsonPropertyName("wind")] public WindDetails Wind { get; set; }
        }

        public class RangeValue
        {
             [JsonPropertyName("low")] public int Low { get; set; }
             [JsonPropertyName("high")] public int High { get; set; }
             [JsonPropertyName("unit")] public string Unit { get; set; }
        }

        public class ForecastText { 
                    [JsonPropertyName("code")]public string Code { get; set; }
                    [JsonPropertyName("text")]public string Text { get; set; }
        }

        public class ValidPeriod { 
                    [JsonPropertyName("start")]public DateTime Start { get; set; }
                    [JsonPropertyName("end")] public DateTime End { get; set; }
                    [JsonPropertyName("text")] public string Text { get; set; }
        }

            public class WindDetails
            {
                [JsonPropertyName("speed")] public RangeValue Speed { get; set; }
                [JsonPropertyName("direction")] public string? Direction { get; set; }
            }

            public class ForecastPeriod
            {
                [JsonPropertyName("timePeriod")] public ValidPeriod TimePeriod { get; set; }
                [JsonPropertyName("regions")] public Dictionary<string, ForecastText> Regions { get; set; }
            }

     
     
 }
