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
    [RoutePrefix("api/speedforce/users")]
    public class UsersController : ApiController
    {

        [Route("{userid}")]
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

        [Route("registerA")]
        [HttpPost]
        public IHttpActionResult RegisterAthlete(UserModel model)
        {
            User userModel = new User();
            Person personModel = new Person();
            Athlete athleteModel = new Athlete();

            //Person Info
            personModel.Email = model.Email;
            personModel.Name = model.Name;
            personModel.Sex = model.Sex;
            personModel.DateBirth = model.DateBirth;
            personModel.CountryName = model.CountryName;
            personModel.CityName = model.CityName;
            personModel.LastName = model.LastName;
            personModel.TelephoneNumber = model.TelephoneNumber;

            //User Info
            userModel.Username = model.Username;
            userModel.Password = model.Password;
            userModel.Email = model.Email;
            userModel.Role = model.Role;

            //Athlete Info
            athleteModel.Username = model.Username;
            athleteModel.Weight = model.Weight;
            athleteModel.Height = model.Height;
            athleteModel.Bicicleta = model.Bicicleta;
            athleteModel.TipoCiclista = model.TipoCiclista;

            //Adding values to Databases
            personModel.AddPerson();
            var response = userModel.AddUser();
            athleteModel.AddAthlete();

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);
        }



    }

  
}
