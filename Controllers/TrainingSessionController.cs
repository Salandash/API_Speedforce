using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_Speedforce.Controllers
{
    [RoutePrefix("api/speedforce/training")]
    public class TrainingSessionController : ApiController
    {
        [Route("{userid}")]
        [HttpGet]
        public IHttpActionResult SessionList(string userid)
        {
            return Ok();
        }
    }
}
