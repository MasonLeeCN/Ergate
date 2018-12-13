using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Ergate;
using Ergate.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Services.AppAuthentication;

namespace TestSample.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : BaseController
    {
        private IEventPublisher publisher;

        public ValuesController(IEventPublisher _publish)
        {
            this.publisher = _publish;
        }

        // GET api/values
        [HttpGet]
        public string Get()
        {
            string ISO8601time = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            return ISO8601time;
        }

        [JwtAuthorize]
        [HttpGet]
        [Route("my")]
        public string[] GetResult()
        {
            return ToResult(new string[] { "value1", "value2" }, System.Net.HttpStatusCode.BadRequest, "错误的参数");
        }


        [HttpPost]
        [Route("v")]
        public dynamic PostObj([FromForm] Persion p)
        {
            string appDefinition = new StreamReader(Request.Body).ReadToEnd();
            string appDefinition2 = new StreamReader(Request.Body).ReadToEnd();
            string appDefinition3 = new StreamReader(Request.Body).ReadToEnd();
            return Json(p);
        }


        [HttpPost]
        [Route("p")]
        public dynamic PostParam([FromForm] string name, [FromForm] int age)
        {
            return true;
        }
    }



    public class Persion : BaseEvent
    {
        public int Age { get; set; }

        public string Name { get; set; }
    }
}
