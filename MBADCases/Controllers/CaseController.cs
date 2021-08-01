using MBADCases.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MBADCases.Models;
using Microsoft.AspNetCore.Authorization;
using MBADCases.Authentication;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MBADCases.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BasicAuthFilter()]
    public class CaseController : ControllerBase
    {
        private readonly CaseService _caseservice;

        public CaseController(CaseService caseservice)
        {
            _caseservice = caseservice;
        }
        // GET: api/<CaseController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CaseController>/5
        [HttpGet("{id:length(24)}", Name = "GetCase")]
        public Case Get(string id)
        {
            var ocase =  _caseservice.Get(id);
           
            if (ocase == null)
            {
                ocase = new Models.Case() { Casetype="",_id="" };
                return ocase;
            }

            return ocase;
        }

        // POST api/<CaseController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CaseController>/5
        [HttpPut("{CaseType}")]
        public CreatedAtRouteResult Put(string CaseType, Case ocase)
        {
            // ocase = new Models.Case() { Casetype = "Dispute", Id = "" };
            if(ocase.Casetype != CaseType) { ocase.Casetype = CaseType; }
            var oretcase= _caseservice.Create(ocase);
            
            return CreatedAtRoute("GetCase", new { id = ocase._id.ToString()  }, ocase);
        }

        // DELETE api/<CaseController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
