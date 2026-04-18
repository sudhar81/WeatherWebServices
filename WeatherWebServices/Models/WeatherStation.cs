using System.Text.Json.Serialization;

namespace WeatherWebServices.Models
{
    public class WeatherStation
    {
        [JsonPropertyName("Id")]
        public string StationId { get; set; } // Matches SQL PRIMARY KEY

        [JsonPropertyName("name")]
        public string StationName { get; set; }
        
       public Location Location { get; set; }   
    }
}
