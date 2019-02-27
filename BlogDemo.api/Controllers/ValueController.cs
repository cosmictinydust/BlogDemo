using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BlogDemo.api.Controllers
{
    [Route("api/values")]
    public class ValueController:Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("hello");
        }
    }
}
