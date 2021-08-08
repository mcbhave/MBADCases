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

        [HttpPut()]
        public IActionResult Put(Adapter adapter)
        {
            string sj = adapter.ToJson();
            
            Message oms;
            var usrid = HttpContext.Session.GetString("mbadtanent");
            try
            {
                _adapterservice.Gettenant(usrid);

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
