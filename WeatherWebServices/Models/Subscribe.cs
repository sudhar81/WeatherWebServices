using System.ComponentModel.DataAnnotations;

namespace WeatherWebServices.Models
{
    public class Subscribe
    {
        public string Email{ get; set; }
        public string Forecastcode { get; set; }
        public int Humidity{ get; set; }

        public int Temperature { get; set; }
        public bool Active { get; set; }

        public DateTime SubscSubscribedDate { get; set; }
    }
}
