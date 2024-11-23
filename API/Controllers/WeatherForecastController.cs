using Microsoft.AspNetCore.Mvc;
using EF.Models;
using Service.Dto.UserManagement;
using Service.UserManagement.Interface;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IRoleService _roleService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRoleService roleService)
        {
            _logger = logger;
            _roleService = roleService;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{                   
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpGet(Name = "GetRoles")]
        public async Task<IEnumerable<RoleDto>> GetRoles()
        {
            return await _roleService.GetAllAsync();
        }
    }
}
