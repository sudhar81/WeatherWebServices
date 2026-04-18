using System.Text.Json.Serialization;

namespace WeatherWebServices.Models
{
    public class WeatherReadingResponseApi
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("data")]
        public  ReadingData Data { get; set; }

        [JsonPropertyName("errorMsg")]
        public string ErrorMsg { get; set; }

     
    }

    public class  ReadingData
    {
        [JsonPropertyName("stations")]
        public List<WeatherStation>? WeatherStations { get; set; }

        [JsonPropertyName("readings")]
        public List<ReadingContainer>? ReadingsValues { get; set; }

        [JsonPropertyName("readingType")]
        public string? ReadingType { get; set; }

        [JsonPropertyName("readingUnit")]
        public string? ReadingUnit { get; set; }
       
    }


    public class ReponseData
    {
        public string StationId { get; set; }
        public decimal Value { get; set; }
        public DateTimeOffset ReadingTimestamp { get; set; }
        public string ReadingType { get; set; }
        public string ReadingUnit { get; set; }
        public string SessionId { get; set; }
    }



    public class ReadingContainer
    {
        [JsonPropertyName("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonPropertyName("data")]
        public List<StationValueDTO> StationValues { get; set; }

        [JsonPropertyName("readingType")]
        public string ReadingType { get; set; }

        [JsonPropertyName("readingUnit")]
        public string ReadingUnit { get; set; }
    }

    public class StationValueDTO
    {
        [JsonPropertyName("stationId")]
        public string StationId { get; set; }

        [JsonPropertyName("value")]
        public decimal Value { get; set; }
       
    }


     
}
