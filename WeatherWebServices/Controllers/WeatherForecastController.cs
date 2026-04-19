using CsvHelper;
using Microsoft.AspNetCore.Authorization;
 
using Microsoft.AspNetCore.Mvc;
 
using System.Globalization;
 
using WeatherWebServices.Data;
using WeatherWebServices.Models;
 

namespace WeatherWebServices.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherForecastService _weatherService;

        private readonly WeatherForecastRepository _repository;
        public WeatherForecastController( WeatherForecastService weatherService,
                      WeatherForecastRepository repository)
        {
             
            _weatherService = weatherService;
            _repository = repository;
        }



        [HttpGet("Current")]
        public async Task<IActionResult> ForecastSync(string? RegionName)
        {
            try
            {

                var response = await _weatherService.SyncWeatherDataAsync();
                if (response != null)
                {
                    if (response.Data.Records.Count > 0) { 
                    var fetchrecord = response.Data.Records[0];
                    var fetchdate = fetchrecord.Date;
                    // fetch from temp table
                    var data = await _repository.GetWeatherForecast(Convert.ToDateTime(fetchdate), RegionName);

                    if (data == null)
                        return NotFound("No Details found");
                    
                    return Ok(data);
                    }
                    else {
                        return NotFound("No Details found");
                    }
                }
                else {
                    return NotFound("No Details found");
                }
                
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404"))
                {
                    return NotFound("No Details found");
                }
                else
                {

                
                    return StatusCode(500, "Internal server error");
                }
            }
        }


        [HttpGet("Date")]
        public async Task<IActionResult> ForecastDate(string ForecastDate, string? RegionName)
        {
            string format = "yyyy-MM-dd";
            try
            {


                // insert to table
               await _weatherService.SyncWeatherDataAsync(ForecastDate);


                // fetch from  table
                var data = await _repository.GetWeatherForecast(Convert.ToDateTime(ForecastDate), RegionName);

                if (data == null)
                    return NotFound("No Details found");

                return Ok(data);
 
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Invalid date"))
                {
                    return StatusCode(500, $"Invalid date: '{ForecastDate}'. Please use the format {format}.");
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


        [HttpGet("export-csv")]
        public async Task<IActionResult> ExportWeatherToCsv(string ForecastDate, string? RegionName)
        {

            // insert to table
            await _weatherService.SyncWeatherDataAsync(ForecastDate);




            // Records
            var records = await _repository.GetWeatherForecastExport(Convert.ToDateTime(ForecastDate), RegionName);

            //   Use a MemoryStream to hold the CSV data in memory
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                //   Write the records to the stream

               
                    csv.WriteRecords( records);
                   await writer.FlushAsync();

                    // 4. Return the file to the browser
                    var result = memoryStream.ToArray();
                    return File(result, "text/csv", $"WeatherExport_{DateTime.Now:yyyyMMdd}.csv");
                
            }
        }



        [HttpPost("Subscribe")]
        public async Task<IActionResult> Subscribe(Subscribe subscribe)
        {
            
            try
            {
                // insert to table
                await _repository.AlertsSubscribe(subscribe);
                return Ok("Subscribed Successfully");

            }
            catch (Exception ex)
            {
                    return StatusCode(500, "error Occured");
                
            }
        }


        [HttpPost("UnSubscribe")]
        public async Task<IActionResult> UnSubscribe(string email)
        {

            try
            {

                // insert to table
                await _repository.AlertsUnSubscribe(email);
                return Ok("Un Subscribed Successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(500, "error Occured");
            }
        }
    }
}
