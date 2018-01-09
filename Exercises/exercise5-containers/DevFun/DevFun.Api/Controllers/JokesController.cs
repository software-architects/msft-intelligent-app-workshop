using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevFun.Common.Services;
using DevFun.Common.Entities;

namespace DevFun.Api.Controllers
{
    [Route("api/[controller]")]
    public class JokesController : Controller
    {
        public IDevJokeService DevJokeService { get; private set; }

        public JokesController(IDevJokeService devJokeService)
        {
            this.DevJokeService = devJokeService ?? throw new ArgumentNullException(nameof(devJokeService));
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<DevJoke>> Get()
        {
            return await this.DevJokeService.GetJokes();
        }

        [HttpGet("random")]
        public async Task<DevJoke> GetRandom()
        {
            return await this.DevJokeService.GetRandomJoke();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<DevJoke> Get(int id)
        {
            return await this.DevJokeService.GetJokeById(id);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]DevJoke value)
        {
            if (value == null)
            {
                return BadRequest("Public key data is null.");
            }

            try
            {
                var joke = await DevJokeService.Create(value);
                if (joke == null) return BadRequest();
                return CreatedAtRoute(new { id = joke.Id }, joke);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]DevJoke value)
        {
            if (value == null || value.Id != id)
            {
                return BadRequest();
            }

            var pkData = await DevJokeService.GetJokeById(id);
            if (pkData == null)
            {
                return NotFound();
            }

            var updatedPkData = await DevJokeService.Update(value);
            return new ObjectResult(updatedPkData);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var joke = await DevJokeService.GetJokeById(id);
            if (joke == null)
            {
                return NotFound();
            }

            await DevJokeService.Delete(id);
            return new NoContentResult();
        }
    }
}
