using Microsoft.AspNetCore.Mvc;
using WebBarg.Application.DTO;
using WebBarg.Application.Interfaces;
using WebBarg.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebBarg.WebApp.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        public async Task<List<UserStatistics>> ChartPie(CancellationToken cancellationToken, string? filter = null)
        {
            var data = await _userService.GetStatisticsAsync(filter, cancellationToken);

            return data;
        }
        [HttpGet]
        public async Task<List<UserDto>> GetUsers(CancellationToken cancellationToken, string? filter = null, int? pageNumber = 1)
        {
            var data = await _userService.GetAllUsersAsync(filter, pageNumber ?? 1, cancellationToken);

            return data.ToList();
        }
    }
}
