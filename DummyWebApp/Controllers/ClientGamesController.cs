using GamesClient;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DummyWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientGamesController : ControllerBase
    {
        private readonly IGameApiClient _gameApiClient;

        public ClientGamesController(IGameApiClient gameApiClient)
        {
            _gameApiClient = gameApiClient;
        }
        // GET: api/<ClientGamesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ClientGamesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ClientGamesController>
        [HttpPost]
        public async Task<ActionResult<bool>> Post()
        {
            await _gameApiClient.AddClientGamesToExistingCompany();
            return Ok(true);
        }

        // PUT api/<ClientGamesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ClientGamesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
