using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_Speedforce.Business;
using API_Speedforce.Models;

namespace API_Speedforce.Controllers
{
    /// <summary>
    /// Controller that handles all requests related to Training Sessions.
    /// </summary>
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        [Route("countries")]
        [HttpGet]
        public IHttpActionResult CountryList()
        {
            var response = Utility.GetCountryList();

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);
        }

        [Route("cities/{country}")]
        [HttpGet]
        public IHttpActionResult CityList(string country)
        {
            var response = Utility.GetCityList(Utility.GetCountryID(country));

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);
        }
    }
}
