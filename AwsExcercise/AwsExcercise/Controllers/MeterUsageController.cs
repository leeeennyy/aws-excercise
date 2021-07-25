using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AwsExcercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterUsageController : ControllerBase
    {
        // GET: api/<MeterUsageController>
        [HttpGet]
        public int GetTotalUsage()
        {
            return 0;
        }
    }
}
