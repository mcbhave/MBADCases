using MBADCases.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MBADCases.Models;
using MBADCases.Authentication;
using MongoDB.Bson;
using System;
using MongoDB.Bson.Serialization;
using Microsoft.AspNetCore.Http;
namespace MBADCases.Controllers
{

    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [BasicAuthFilter()]
    public class CaseTypeController : ControllerBase
    {
        private readonly CaseTypeService _casetypeservice;
        public CaseTypeController(CaseTypeService casetypeservice)
        {
            _casetypeservice = casetypeservice;
        }
        [HttpGet("{id:length(24)}", Name = "GetCaseType")]
        public IActionResult Get(string id)
        {
            Message oms;
            var usrid = HttpContext.Session.GetString("mbaduserid");
            var tenantid = HttpContext.Session.GetString("mbadtanent");
            try
            {
                _casetypeservice.Gettenant(tenantid);

                CaseType ocase = _casetypeservice.Get(id);
                oms = _casetypeservice.SetMessage(id, id, "GET", "200", "Case type Search", usrid, null);
                if (ocase == null)
                {
                    ocase = new CaseType();
                    oms = _casetypeservice.SetMessage(ocase._id, id, "GET", "400", "Not found", usrid, null);
                    ocase.Message = new MessageResponse() { Messagecode = oms.Messagecode,  Messageype = oms.Messageype, _id = oms._id };
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, ocase);
                }
                else
                {
                    oms = _casetypeservice.SetMessage(ocase._id, id, "GET", "200", "Case type Search by name", usrid, null);
                    ocase.Message = new MessageResponse() { Messagecode = oms.Messagecode,  Messageype = oms.Messageype, _id = oms._id };
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, ocase);
                }
            }
            catch (Exception ex)
            {
                CaseType ocase = new CaseType();
                ocase._id = id;
                oms = _casetypeservice.SetMessage(id, id, "GET", "501", "Case Type Search", usrid, ex);

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, new CaseTypeResponse(ocase, oms));
            }
        }

        [HttpGet("{name}", Name = "GetCaseTypeByName")]
        public IActionResult GetByName(string name)
        {
            Message oms;
            var usrid = HttpContext.Session.GetString("mbaduserid");
            var tenantid = HttpContext.Session.GetString("mbadtanent");
            try
            {
                _casetypeservice.Gettenant(tenantid);

                CaseType ocase = _casetypeservice.GetByName(name);
               
                if (ocase == null)
                {
                    ocase = new CaseType();
                    oms = _casetypeservice.SetMessage(ocase._id, name, "GET", "400", "Not found", usrid, null);
                    ocase.Message = new MessageResponse() { Messagecode = oms.Messagecode,  Messageype = oms.Messageype, _id = oms._id };
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, ocase);
                }
                else
                {
                    oms = _casetypeservice.SetMessage(ocase._id, name, "GET", "200", "Case type Search by name", usrid, null);
                    ocase.Message = new MessageResponse() { Messagecode = oms.Messagecode,  Messageype = oms.Messageype, _id = oms._id };
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, ocase);
                }

            }
            catch (Exception ex)
            {
                CaseType ocaset= new CaseType();
                
                oms = _casetypeservice.SetMessage(name, name, "GET", "501", "Case Type Search", usrid, ex);

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, new CaseTypeResponse(ocaset, oms));
            }
        }
        [HttpPost("{id:length(24)}", Name = "UpdateCaseType")]
        public IActionResult Post(string id, CaseType ocasetype)
        {
            Message oms;
            var usrid = HttpContext.Session.GetString("mbaduserid");
            var tenantid = HttpContext.Session.GetString("mbadtanent");
            //string id = ocase._id;
            ocasetype._id = id;
            try
            {
                _casetypeservice.Gettenant(tenantid);

                _casetypeservice.Update(id, ocasetype);
                oms = _casetypeservice.SetMessage( id, null, "POST", "UPDATE", "Case type update", usrid, null);
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, new CaseTypeResponse(ocasetype, oms));
            }
            catch (Exception ex)
            {
                oms = _casetypeservice.SetMessage( id, null, "POST", "UPDATE", "Case type update", usrid, ex);
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, new CaseTypeResponse(ocasetype, oms));
            }
        }

        [HttpPut("{CaseTypeName}")]
        public IActionResult Put(string CaseTypeName,CaseType ocasetype)
        {
            Message oms;
            var usrid = HttpContext.Session.GetString("mbaduserid");
            var tenantid = HttpContext.Session.GetString("mbadtanent");
            string createuserid = ocasetype.Createuser;
            try
            {
                _casetypeservice.Gettenant(tenantid);
               
              
                var oretcase = _casetypeservice.Create(CaseTypeName,ocasetype);
                oms = _casetypeservice.SetMessage(oretcase._id, CaseTypeName, "PUT", "200", "Case type insert", createuserid, null);

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, new CaseTypeResponse(ocasetype, oms));
            }
            catch (Exception ex)
            {
                oms = _casetypeservice.SetMessage(null, CaseTypeName, "PUT", "", "Case insert", createuserid, ex);
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, new CaseTypeResponse(ocasetype, oms));

            }

        }
    }
}
