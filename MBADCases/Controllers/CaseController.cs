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
                _caseservice.Gettenant(usrid);

                Case ocase = _caseservice.Get(id);
                oms = _caseservice.SetMessage(ICaseTypes.CASE, id, id, "GET", "200", "Case Searched", usrid, null);
                if (ocase == null)
                {
                    ocase = new Case();
                }
                ocase.Message = new MessageResponse() { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, ocase);
            }
            catch (Exception ex)
            {
                Case ocase = new Case();
                ocase._id = id;
                oms = _caseservice.SetMessage(ICaseTypes.CASE, id, id, "GET", "", "", usrid, ex);
 
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, new CaseResponse(ocase, oms));
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
                _caseservice.Gettenant(usrid);

                Case ocase = _caseservice.Get(id);
                oms = _caseservice.SetMessage(ICaseTypes.CASE, id, id, "GET", "200", "Case Searched", usrid, null);

                ocase.Message = new MessageResponse() { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, ocase);
            }
            catch (Exception ex)
            {
                Case ocase = new Case();
                ocase._id = id;
                oms = _caseservice.SetMessage(ICaseTypes.CASE, id, id, "GET", "", "", usrid, ex);
 
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, new CaseResponse(ocase, oms));
            }

        }
        // POST api/<CaseController>
        [MapToApiVersion("1.0")]
        [HttpPost]
        public IActionResult Post(Case ocase)
        {
            Message oms;
            var usrid = HttpContext.Session.GetString("mbadtanent");
            string sj = ocase.Caseattributes.ToJson();
            string id = ocase._id;
            try
            {
                _caseservice.Gettenant(usrid);

                _caseservice.Update(ocase._id, ocase.Caseattributes);
                  oms = _caseservice.SetMessage(ICaseTypes.CASE, ocase._id,sj, "POST", "UPDATE", "Case update", usrid, null);
                 return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, new CaseResponse(ocase, oms));
            }
            catch(Exception ex)
            {
                 
                oms = _caseservice.SetMessage(ICaseTypes.CASE, id, sj, "POST", "UPDATE", "Case update", usrid, ex);
                 return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, new CaseResponse(ocase, oms));
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
            string createuserid = ocase.Createuser;
            try
            {
                _caseservice.Gettenant(usrid);

                if (ocase.Casetype != CaseType) { ocase.Casetype = CaseType; }
                if (ocase.Updateuser == null) { ocase.Updateuser = createuserid; }
                if (ocase.Createdate == null) { ocase.Createdate = DateTime.UtcNow.ToString(); }
                if (ocase.Updatedate == null) { ocase.Updatedate = DateTime.UtcNow.ToString(); }
                var oretcase = _caseservice.Create(ocase);
                oms = _caseservice.SetMessage(ICaseTypes.CASE, oretcase._id, sj, "PUT", "200", "Case insert", createuserid, null);

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, new CaseResponse(ocase, oms));
            }
            catch(Exception ex)
            {
                oms = _caseservice.SetMessage(ICaseTypes.CASE, id, sj, "PUT", "", "Case insert", createuserid, ex);
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, new CaseResponse(ocase, oms));
                 
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
                _caseservice.Gettenant(usrid);

                _caseservice.Remove(id);
                oms = _caseservice.SetMessage(ICaseTypes.CASE, id, id, "DELETE", "200", "Case delete", usrid, null);
                 return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, new CaseResponse(ocase, oms));
            }
            catch (Exception ex)
            {
                oms = _caseservice.SetMessage(ICaseTypes.CASE, id, id, "DELETE", "200", "Case delete", usrid, ex);
                 return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, new CaseResponse(ocase, oms));

            }
        }
    }
}
