using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MBADCases.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MBADCases.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    //[BasicAuthFilter()]
    public class SearchCasesController : ControllerBase
    {
        // GET: api/<SearchCasesController>
        [HttpGet]
        public string Get()
        {
            List<Case> colcases = new List<Case>();
            Case ocase = new Case() { _id = Guid.NewGuid().ToString(), Casetype = "Dispute", Casetitle = "MB Test 1" };
            colcases.Add(ocase);
            ocase = new Case() { _id = Guid.NewGuid().ToString(), Casetype = "Dispute", Casetitle = "MB Test 2" };
            colcases.Add(ocase);
            ocase = new Case() { _id = Guid.NewGuid().ToString(), Casetype = "Dispute", Casetitle = "MB Test 3" };
            colcases.Add(ocase);
            ocase = new Case() { _id = Guid.NewGuid().ToString(), Casetype = "Dispute", Casetitle = "MB Test 4" };
            colcases.Add(ocase);
            return Newtonsoft.Json.JsonConvert.SerializeObject( colcases);
        }

       
        // GET api/<SearchCasesController>/5
        [HttpGet("{searchtext}")]
        public List<Case> Get(string searchtext)
        {
            List<Case> colcases = new List<Case>();
            Case ocase = new Case() { _id = Guid.NewGuid().ToString(), Casetype = "Dispute", Casetitle = "MB Test 1",Casedescription="Case description 1" };
            colcases.Add(ocase);
            ocase = new Case() { _id = Guid.NewGuid().ToString(), Casetype = "Dispute", Casetitle = "MB Test 2", Casedescription = "Case description 2" };
            colcases.Add(ocase);
            ocase = new Case() { _id = Guid.NewGuid().ToString(), Casetype = "Dispute", Casetitle = "MB Test 3", Casedescription = "Case description 3" };
            colcases.Add(ocase);
            ocase = new Case() { _id = Guid.NewGuid().ToString(), Casetype = "Dispute", Casetitle = "MB Test 4", Casedescription = "Case description 4" };
            colcases.Add(ocase);
            return colcases;
        }

        // POST api/<SearchCasesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SearchCasesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SearchCasesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
