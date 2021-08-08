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
    public class AdapterController : ControllerBase
    {
        private readonly AdapterService _adapterservice;
        public AdapterController(AdapterService adapterser)
        {
            _adapterservice = adapterser;
        }
        [HttpGet("{id:length(24)}", Name = "GetAdapter")]
        public IActionResult Get(string id)
        {
            Message oms;
            var usrid = HttpContext.Session.GetString("mbadtanent");
            try
            {
                _adapterservice.Gettenant(usrid);

                Adapter ocase = _adapterservice.Get(id);
                oms = _adapterservice.SetMessage(id, id, "GET", "200", "Case type Search", usrid, null);
                if (ocase == null)
                {
                    ocase = new Adapter();
                    oms = _adapterservice.SetMessage(ocase._id, id, "GET", "400", "Not found", usrid, null);
                    ocase.Message = new MessageResponse() { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, ocase);
                }
                else
                {
                    oms = _adapterservice.SetMessage(ocase._id, id, "GET", "200", "Case type Search by name", usrid, null);
                    ocase.Message = new MessageResponse() { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, ocase);
                }
            }
            catch (Exception ex)
            {
                CaseType ocase = new CaseType();
                ocase._id = id;
                oms = _adapterservice.SetMessage(id, id, "GET", "501", "Case Type Search", usrid, ex);

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, new CaseTypeResponse(ocase, oms));
            }
        }

        [HttpGet("{name}", Name = "GetAdapterByName")]
        public IActionResult GetByName(string name)
        {
            Message oms;
            var usrid = HttpContext.Session.GetString("mbadtanent");
            try
            {
                _adapterservice.Gettenant(usrid);

                Adapter ocase = _adapterservice.GetByName(name);

                if (ocase == null)
                {
                    ocase = new Adapter();
                    oms = _adapterservice.SetMessage(ocase._id, name, "GET", "400", "Not found", usrid, null);
                    ocase.Message = new MessageResponse() { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, ocase);
                }
                else
                {
                    oms = _adapterservice.SetMessage(ocase._id, name, "GET", "200", "Case type Search by name", usrid, null);
                    ocase.Message = new MessageResponse() { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, ocase);
                }

            }
            catch (Exception ex)
            {
                Adapter ocaset = new Adapter();

                oms = _adapterservice.SetMessage(name, name, "GET", "501", "Case Type Search", usrid, ex);
                ocaset.Message = new MessageResponse() { Messagecode = oms.Messagecode, MessageDesc = oms.MessageDesc, Messageype = oms.Messageype, _id = oms._id };
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, ocaset);
            }
        }
        
        [HttpPost("{id:length(24)}", Name = "UpdateAdapter")]
        public IActionResult Post(string id, Adapter ocaseadaptr)
        {
            Message oms;
            var usrid = HttpContext.Session.GetString("mbadtanent");
            //string id = ocase._id;
            ocaseadaptr._id = id;
            try
            {
                _adapterservice.Gettenant(usrid);

                _adapterservice.Update(id, ocaseadaptr);
                oms = _adapterservice.SetMessage(id, null, "POST", "UPDATE", "Case type update", usrid, null);
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, new CaseResponse(ocaseadaptr._id, oms));
            }
            catch (Exception ex)
            {
                oms = _adapterservice.SetMessage(id, null, "POST", "UPDATE", "Case type update", usrid, ex);
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, new CaseResponse(ocaseadaptr._id, oms));
            }
        }

        [HttpPut()]
        public IActionResult Put(Adapter adapter)
        {
            string sj = adapter.ToJson();
            
            Message oms;
            var usrid = HttpContext.Session.GetString("mbadtanent");
            try
            {
                _adapterservice.Gettenant(usrid);
                if(adapter.Name==null || adapter.Name == "")
                {
                    adapter.Name="Adapter_" + helperservice.RandomString(5, false);
                }
                var oretcase = _adapterservice.Create(adapter);
                oms = _adapterservice.SetMessage( oretcase._id, sj, "PUT", "200", "Case insert", usrid, null);

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, new CaseResponse(oretcase._id, oms));
            }
            catch (Exception ex)
            {
                oms = _adapterservice.SetMessage("", sj, "PUT", "", "Case insert", usrid, ex);
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, new CaseResponse(adapter._id, oms));

            }

        }
    }
}
