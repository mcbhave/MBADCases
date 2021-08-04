using MBADCases.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MBADCases.Models;
using MBADCases.Authentication;
using MongoDB.Bson;
using System;
using MongoDB.Bson.Serialization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MBADCases.Controllers
{
  
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [BasicAuthFilter()]
    public class CaseController : ControllerBase
    {
        private const string V = "1.0";
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
        [MapToApiVersion("1.0")]
        [HttpGet("{id:length(24)}", Name = "GetCase")]
        public IActionResult Get(string id)
        {
            Case ocase =  _caseservice.Get(id);

            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, ocase);
        }
        [MapToApiVersion("2.0")]
        [HttpGet("{id:length(24)}", Name = "GetCase")]
        public IActionResult Get2(string id)
        {
            Case ocase = _caseservice.Get(id);

            //if (ocase == null)
            //{
            //    ocase = new Case { };
            //    return ocase;
            //}
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, ocase);
             
        }
        // POST api/<CaseController>
        [MapToApiVersion("1.0")]
        [HttpPost]
        public IActionResult Post(Case value)
        {
            _caseservice.Update(value._id,value.Caseattributes);
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, null);
        }

        // PUT api/<CaseController>/5
        [HttpPut("{CaseType}")]
        public CreatedAtRouteResult Put(string CaseType, Case ocase)
        {
            if (ocase.Casetype != CaseType) { ocase.Casetype = CaseType; }
            var oretcase = _caseservice.Create(ocase);
            return CreatedAtRoute("GetCase", new { id = ocase._id.ToString()  }, ocase);
        }

        // DELETE api/<CaseController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
              _caseservice.Remove(id);
        }
    }
}
