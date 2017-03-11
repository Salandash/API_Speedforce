using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.ApplicationInsights;
using Speedforce_DataAccess;
using System.Threading;
using System.Security.Principal;
using Newtonsoft.Json;
using API_Speedforce.Models;


namespace API_Speedforce.Controllers
{
    [RoutePrefix("api/speedforce")]
    public class UsersController : ApiController
    {

        [Route("user/{userid}")]
        [HttpGet]
        public IHttpActionResult UserInfo(string userid)
        {
            User userModel = new User(userid);

            var response = userModel.GetUser(userid);

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);
        }

        [Route("loginA")]
        [HttpPost]
        public IHttpActionResult AthleteLogin (User model)
        {
            User userModel = new User();
            userModel.Username = model.Username;
            userModel.Password = model.Password;

            var response = userModel.VerifyLogin();

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);
        }
    }

  
}
