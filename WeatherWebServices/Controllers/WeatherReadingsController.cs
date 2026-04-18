using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WeatherWebServices.Data;
 
namespace WeatherWebServices.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherReadingsController : ControllerBase
    {

        private readonly WeatherReadingsService _weatherService;

        private readonly WeatherReadingsRepository _repository;
//        public WeatherController(WeatherService weatherService) => _weatherService = weatherService;

        public WeatherReadingsController(WeatherReadingsRepository repository, WeatherReadingsService weatherService)
        {
            _repository = repository;
            _weatherService = weatherService;
        }




        [HttpGet("WeatherStation")]
        public async Task<IActionResult> GetWeatherStations()
        {
            try
            {
               
 

                // fetch from temp table
                var data = await _repository.GetAllWeatherStations();

                if (data == null || !data.Any())
                    return NotFound("No Details found");

                return Ok(data);
            }
            catch (Exception ex)
            {
             
                return StatusCode(500, "Internal server error");
            }
        }
        

 
 

        [HttpGet("Current")]
        public async Task<IActionResult> GetWeatherDetails(string? StationId)
        {
            try
            {
                Random rng = new Random();
                int startNo = 1000;
                int endNo = 9999;

                string sResult2 = rng.Next(startNo, endNo).ToString();

                string SessionId = "SS" + DateTime.Now.ToString("yyyyMMddHHmmss") + sResult2;


                
                // insert temp table 
                await _weatherService.FetchLatestWeatherReadings(SessionId,"");

                // fetch from temp table
               var data = await _repository.GetLatestWeatherDetails(SessionId,StationId);

                if (data == null || !data.Any())
                    return NotFound("No Details found");

                return Ok(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetLatestWeatherDetails" + ex.ToString);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("History")]
        public async Task<IActionResult> GetWeatherDetailsHistory(string Date,string? StationId)
        {
            string format = "yyyy-MM-dd";
            try
            {
                Random rng = new Random();
                int startNo = 1000;
                int endNo = 9999;

                string sResult2 = rng.Next(startNo, endNo).ToString();

                string SessionId = "SS" + DateTime.Now.ToString("yyyyMMddHHmmss") + sResult2;

               


                // insert  table 
                await _weatherService.FetchLatestWeatherReadings(SessionId,Date);

                // fetch from table
                var data = await _repository.GetLatestWeatherDetails(SessionId, StationId);

                if (data == null || !data.Any())
                    return NotFound("No Details found");

                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Invalid date"))
                {
                    return StatusCode(500, $"Invalid date: '{Date}'. Please use the format {format}.");
                }
                else if (ex.Message.Contains("404"))
                {
                    return NotFound("No Details found");
                }
                else { 

                    Console.WriteLine("GetLatestWeatherDetails" + ex.ToString);
                return StatusCode(500, "Internal server error");
                }
            }
        }



        [HttpGet("export-csv")]
        public async Task<IActionResult> ExportWeatherToCsv(string Date, string? StationId)
        {
            string format = "yyyy-MM-dd";
            try
            {

                Random rng = new Random();
                int startNo = 1000;
                int endNo = 9999;

                string sResult2 = rng.Next(startNo, endNo).ToString();

                string SessionId = "SS" + DateTime.Now.ToString("yyyyMMddHHmmss") + sResult2;


                // insert  table 
                await _weatherService.FetchLatestWeatherReadings(SessionId, Date);

                // fetch from table
                var data = await _repository.GetLatestWeatherDetails(SessionId, StationId);

                //   Use a MemoryStream to hold the CSV data in memory
                using (var memoryStream = new MemoryStream())
                using (var writer = new StreamWriter(memoryStream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    //   Write the records to the stream


                    csv.WriteRecords(data);
                    await writer.FlushAsync();

                    // 4. Return the file to the browser
                    var result = memoryStream.ToArray();
                    return File(result, "text/csv", $"WeatherExport_{DateTime.Now:yyyyMMdd}.csv");

                }
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Invalid date"))
                {
                    return StatusCode(500, $"Invalid date: '{Date}'. Please use the format {format}.");
                }
                else if (ex.Message.Contains("404"))
                {
                    return NotFound("No Details found");
                }
                else
                {

                   
                    return StatusCode(500, "Internal server error");
                }
            }
        }


    }
}
