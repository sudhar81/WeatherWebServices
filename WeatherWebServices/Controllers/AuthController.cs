using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WeatherWebServices.Data;
using WeatherWebServices.Models;

namespace WeatherWebServices.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("api/auth")]
    public class AuthController(TokenService tokenService) : ControllerBase
    {

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login request)
        {
            // Replace this with real database validation
            if (request.Username == "admin" && request.Password == "password123")
            {
                var token = tokenService.GenerateToken(request.Username, "Admin");
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }
    }
}
