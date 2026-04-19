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

        [HttpPost("Token")]
        public IActionResult Login([FromBody] Login request)
        {
           
            if (request.Username == "admin" && tokenService.EncryptPassword(request.Password) == "cGFzc3dvcmQxMjN0RW1wYXR1cmVfVEVzdF9QQXNzd29yZA==")
            {
                var token = tokenService.GenerateToken(request.Username, "Admin");
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }


    }
}
