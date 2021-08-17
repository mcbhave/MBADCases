using MBADCases.Services;
using Microsoft.AspNetCore.Mvc;
using MBADCases.Models;
using MBADCases.Authentication;
using System;
using Microsoft.AspNetCore.Http;
namespace MBADCases.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [BasicAuthFilter()]
    public class TenantController : ControllerBase
    {
        //private const string V = "1.0";
        private readonly TenantService _tenantservice;
       
        public TenantController(TenantService tenantservice)
        {
            _tenantservice = tenantservice;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var usrid = HttpContext.Session.GetString("mbadtanent");
            try
            {
                _tenantservice.Gettenant(usrid);
                Tenant ocase = _tenantservice.Get();
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, ocase);
            }
            catch { throw; }
         }
        // GET api/<CaseController>/5
        [MapToApiVersion("1.0")]
        [HttpGet("{id:length(24)}", Name = "GetTenant")]
        public IActionResult Get(string id)
        {
            Message oms;
            var usrid = HttpContext.Session.GetString("mbadtanent");
            try
            {
                _tenantservice.Gettenant(usrid);

                Tenant ocase = _tenantservice.Get(id);
                oms = _tenantservice.SetMessage(id, id, "GET", "200", "Case type Search", usrid, null);
                if (ocase == null)
                {
                    ocase = new Tenant();
                    oms = _tenantservice.SetMessage(ocase._id, id, "GET", "400", "Not found", usrid, null);
                    ocase.Message = new MessageResponse() { Messagecode = oms.Messagecode, Messageype = oms.Messageype, _id = oms._id };
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, ocase);
                }
                else
                {
                    oms = _tenantservice.SetMessage(ocase._id, id, "GET", "200", "Case type Search by name", usrid, null);
                    ocase.Message = new MessageResponse() { Messagecode = oms.Messagecode, Messageype = oms.Messageype, _id = oms._id };
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, ocase);
                }
            }
            catch (Exception ex)
            {
                Tenant ocase = new Tenant();
                ocase._id = id;
                oms = _tenantservice.SetMessage(id, id, "GET", "501", "Case Type Search", usrid, ex);

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, ocase);
            }
        }
         
        // POST api/<CaseController>
        [MapToApiVersion("1.0")]
        [HttpPost("{id:length(24)}", Name = "UpdateTenant")]
        public IActionResult Post(string id, Tenant tenant)
        {
            Message oms;
            var usrid = HttpContext.Session.GetString("mbadtanent");
            //string id = ocase._id;
            //ocasetype._id = id;
            try
            {
                _tenantservice.Gettenant(usrid);

                _tenantservice.Update(id, tenant);
                oms = _tenantservice.SetMessage(id, null, "POST", "UPDATE", "Case type update", usrid, null);
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, tenant);
            }
            catch (Exception ex)
            {
                oms = _tenantservice.SetMessage(id, null, "POST", "UPDATE", "Case type update", usrid, ex);
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, tenant);
            }

        }
         
        // PUT api/<CaseController>/5
        [HttpPut]
        public IActionResult Put(Tenant tenant)
        {
            Message oms;
            var usrid = HttpContext.Session.GetString("mbadtanent");
            string createuserid = tenant.Createuser;
            try
            {
                _tenantservice.Gettenant(usrid);
                var oretcase = _tenantservice.Create(tenant);
                oms = _tenantservice.SetMessage(oretcase._id, "", "PUT", "200", "Tenant insert", createuserid, null);

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, tenant);
            }
            catch (Exception ex)
            {
                oms = _tenantservice.SetMessage(null, "", "PUT", "", "Tenant insert", createuserid, ex);
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, tenant);

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
                _tenantservice.Remove(id);
                oms = _tenantservice.SetMessage( id, id, "DELETE", "200", "Case delete", usrid, null);
            
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, id);
            }
            catch (Exception ex)
            {
                oms = _tenantservice.SetMessage(  id, id, "DELETE", "200", "Case delete", usrid, ex);
                 
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status417ExpectationFailed, ocase);

            }
        }

    }
}
