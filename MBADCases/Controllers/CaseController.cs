using MBADCases.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MBADCases.Models;
using MBADCases.Authentication;
using MongoDB.Bson;
using System;
using MongoDB.Bson.Serialization;
using Microsoft.AspNetCore.Http;
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
            Message oms;
            var usrid = HttpContext.Session.GetString("mbadtanent");
            try
            {
                Case ocase = _caseservice.Get(id);
                oms = _caseservice.SetMessage(ocase, id, id, "GET", "200", "Case Searched", usrid, null);
                ocase.Message = new ReturnMessage { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, ocase);
            }
            catch (Exception ex)
            {
                Case ocase = new Case();
                oms = _caseservice.SetMessage(ocase, id, id, "GET", "", "", usrid, ex);
                ocase.Message = new ReturnMessage { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, ocase);
            }
        }
        [MapToApiVersion("2.0")]
        [HttpGet("{id:length(24)}", Name = "GetCase")]
        public IActionResult Get2(string id)
        {
            Message oms;
            var usrid = HttpContext.Session.GetString("mbadtanent");
            try
            {
                Case ocase = _caseservice.Get(id);
                oms = _caseservice.SetMessage(ocase, id, id, "GET", "200", "Case Searched", usrid, null);
                ocase.Message = new ReturnMessage { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, ocase);
            }
            catch (Exception ex)
            {
                Case ocase = new Case();
                oms = _caseservice.SetMessage(ocase, id, id, "GET", "", "", usrid, ex);
                ocase.Message = new ReturnMessage { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, ocase);
            }

        }
        // POST api/<CaseController>
        [MapToApiVersion("1.0")]
        [HttpPost]
        public IActionResult Post(Case value)
        {
            Message oms;
            var usrid = HttpContext.Session.GetString("mbadtanent");
            string sj = value.Caseattributes.ToJson();
            string id = value._id;
            try
            {
              
                _caseservice.Update(value._id, value.Caseattributes);
                  oms = _caseservice.SetMessage(value, value._id,sj, "POST", "UPDATE", "Case update", usrid, null);
                value.Message = new ReturnMessage { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, value);
            }
            catch(Exception ex)
            {
                Case ocase=new Case(); 

                oms = _caseservice.SetMessage(ocase,id, sj, "POST", "UPDATE", "Case update", usrid, ex);
                ocase.Message = new ReturnMessage { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, ocase);
            }
          
        }

        // PUT api/<CaseController>/5
        [HttpPut("{CaseType}")]
        public IActionResult Put(string CaseType, Case ocase)
        {
            Message oms;
            var usrid = HttpContext.Session.GetString("mbadtanent");
            string sj = ocase.Caseattributes.ToJson();
            string id = ocase._id;
            try
            {
                if (ocase.Casetype != CaseType) { ocase.Casetype = CaseType; }
                var oretcase = _caseservice.Create(ocase);
                oms = _caseservice.SetMessage(ocase, id, sj, "PUT", "200", "Case insert", usrid, null);
                ocase.Message = new ReturnMessage { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };
                return CreatedAtRoute("GetCase", new { id = ocase._id.ToString() }, ocase);
            }
            catch(Exception ex)
            {
                oms = _caseservice.SetMessage(ocase, id, sj, "PUT", "", "Case insert", usrid, ex);
                ocase.Message = new ReturnMessage { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, ocase);
                 
            }
           
        }

        // DELETE api/<CaseController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            Message oms;
            Case ocase = new Case();
            var usrid = HttpContext.Session.GetString("mbadtanent");
            //string sj = ocase.Caseattributes.ToJson();
           // string id = ocase._id;
            try
            {
                _caseservice.Remove(id);
                oms = _caseservice.SetMessage(ocase, id, id, "DELETE", "200", "Case delete", usrid, null);
                ocase.Message = new ReturnMessage { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, ocase);
            }
            catch (Exception ex)
            {
                oms = _caseservice.SetMessage(ocase, id, id, "DELETE", "200", "Case delete", usrid, ex);
                ocase.Message = new ReturnMessage { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, ocase);

            }
        }
    }
}
