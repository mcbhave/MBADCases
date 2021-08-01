using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MBADCases.Models;
using System.Net.Http;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MBADCases.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class provisionController : ControllerBase
    {
        // GET: api/<provisionController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<provisionController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<provisionController>
        [HttpPost]
        public IActionResult Post(WixDB.provision value)
        {
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK,"{}");
             
        }

        // PUT api/<provisionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<provisionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
