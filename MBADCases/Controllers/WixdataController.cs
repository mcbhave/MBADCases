using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MBADCases.Models;
using static MBADCases.Models.WixDB;

namespace MBADCases.Controllers
{
    [Route("api/data")]
    [ApiController]
    public class WixdataController : ControllerBase
    {
        [Route("insert")]
        [Route("insert/{id?}")]
        [HttpPost]
        public IActionResult data(WixDB.data id)
        {
            DataItem oi = new DataItem();
            WixDB.item oitem = new WixDB.item() { _id = Guid.NewGuid().ToString(), _owner = Guid.NewGuid().ToString(), make = "Toyota", model = "Camry", year = 2018, date_added = DateTime.Now.ToLongDateString() } ;
            oi.item = oitem;
        
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, oi);
        }

        [Route("get")]
        [Route("get/{id?}")]
        [HttpPost]
        public IActionResult getitem(WixDB.data id)
        {
            DataItem oi = new DataItem();
            WixDB.item oitem = new WixDB.item() { _id = Guid.NewGuid().ToString(), _owner = Guid.NewGuid().ToString(), make = "Toyota", model = "Camry", year = 2018, date_added = DateTime.Now.ToLongDateString() };
            oi.item = oitem;
            
            
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, oi);
        }
        [Route("find")]
        [Route("find/{id?}")]
        [HttpPost]
        public IActionResult finditem(WixDB.data id)
        {
            
            FindItems olistdata = new FindItems();
            Guid g = Guid.NewGuid();
          
 
            WixDB.item oitem = new WixDB.item() { _id =   Guid.NewGuid().ToString(), _owner = Guid.NewGuid().ToString(), make = "Toyota", model = "Camry", year = 2018, date_added = DateTime.Now.ToString("mmm dd, yyyy hh:mm tt") };
            olistdata.items.Add(oitem);
            oitem = new WixDB.item() { _id =   Guid.NewGuid().ToString(), _owner = Guid.NewGuid().ToString(), make = "Ford", model = "Mustang", year = 2018, date_added = DateTime.Now.ToLongDateString() };
            olistdata.items.Add(oitem);
            oitem = new WixDB.item() { _id = Guid.NewGuid().ToString(), _owner = Guid.NewGuid().ToString(), make = "Tesla", model = "ES", year = 2018, date_added = DateTime.Now.ToLongDateString() };
            olistdata.items.Add(oitem);

            olistdata.totalCount = olistdata.items.Count;
             
            
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, olistdata);
        }

        [Route("update")]
        [Route("update/{id?}")]
        [HttpPost]
        public IActionResult updateitem(WixDB.data id)
        {
            DataItem oi = new DataItem();
            WixDB.item oitem = new WixDB.item() { _id = Guid.NewGuid().ToString(), _owner = Guid.NewGuid().ToString(), model = "Camry" };
           
            oi.item = oitem;
           
          
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, oi);
        }

        [Route("remove")]
        [Route("remove/{id?}")]
        [HttpPost]
        public IActionResult removeitem(WixDB.data id)
        {
            DataItem oi = new DataItem();
            WixDB.item oitem = new WixDB.item() { _id = Guid.NewGuid().ToString(), _owner = Guid.NewGuid().ToString(), make = "Toyota", model = "Camry", year = 2018, date_added = DateTime.Now.ToString("MM-DD-YYYY HH:mm:ss") };
            oi.item = oitem;
          
            
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, oi);
        }

        [Route("count")]
        [Route("count/{id?}")]
        [HttpPost]
        public IActionResult countitem(WixDB.data id)
        {
            DataCount ocount = new DataCount();
            ocount.totalCount = 50;
          

           
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, ocount);
        }
    }
}
