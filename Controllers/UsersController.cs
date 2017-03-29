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

        /// <summary>
        /// Function that fetches user information.
        /// </summary>
        /// <param name="userid">Identifier that lets the function know which user infor will be fetching</param>
        /// <returns></returns>
        [Route("user/{userid}")]
        [HttpGet]
        public IHttpActionResult GetUserInfo(string userid)
        {
            User userModel = new User(userid);

            var response = userModel.GetUser(userid);

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Function to verify an athlete logs in into the system.
        /// </summary>
        /// <param name="model">All the atlhlete's attributes from a json file</param>
        /// <returns></returns>
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

        /// <summary>
        /// Function to register a new athlete to the system.
        /// </summary>
        /// <param name="model">All the atlhlete's attributes from a json file</param>
        /// <returns></returns>
        [Route("registerA")]
        [HttpPost]
        public IHttpActionResult RegisterAthlete(AthleteUser model)
        {
            User userModel = new User();
            Person personModel = new Person();
            Athlete athleteModel = new Athlete();

            #region Model to model - Person
            personModel.Email = model.Email;
            personModel.Name = model.Name;
            personModel.Sex = personModel.GetSexID(model.Sex);
            personModel.BirthDate = model.BirthDate;
            personModel.CountryName = model.CountryName;
            personModel.CityName = model.CityName;
            personModel.LastName = model.LastName;
            personModel.TelephoneNumber = model.TelephoneNumber;
            #endregion

            #region Model to model - User
            userModel.Username = model.Username;
            userModel.Password = model.Password;
            userModel.Email = model.Email;
            userModel.Role = model.Role;
            #endregion

            #region Model to model - Athlete
            athleteModel.Username = model.Username;
            athleteModel.Weight = model.Weight;
            athleteModel.Height = model.Height;
            athleteModel.BikerType = athleteModel.GetBikerTypeID(model.BikerType);
            #endregion

            //Adding values to Databases
            personModel.AddPerson();
            var response = userModel.AddUser();
            athleteModel.AddAthlete();

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Function that get all the athlete's information.
        /// </summary>
        /// <param name="userid">Unique ID of the user/athlete</param>
        /// <returns></returns>
        [Route("athlete/{userid}")]
        [HttpGet]
        public IHttpActionResult GetAthleteInfo(string userid)
        {

            Athlete athleteModel = new Athlete(userid);

            var response = athleteModel.GetAthlete(userid);

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Function that get all the person's information.
        /// </summary>
        /// <param name="userid">Unique ID of the person</param>
        /// <returns></returns>
        [Route("person/{userid}")]
        [HttpGet]
        public IHttpActionResult GetPersonInfo(string userid)
        {
            Person personModel = new Person(userid);

            var response = personModel.GetPerson(userid);

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);
        }



        /// <summary>
        /// Function that updates all of the person's information.
        /// </summary>
        /// <param name="model">A full form o</param>
        /// <returns></returns>
        [Route("updateA")]
        [HttpPost]
        public IHttpActionResult UpdateUser(AthleteUser model)
        {
            User userModel = new User();
            Person personModel = new Person();
            Athlete athleteModel = new Athlete();

            #region Model to model - Person
            personModel.Email = model.Email;
            personModel.Name = model.Name;
            personModel.Sex = personModel.GetSexID(model.Sex);
            personModel.BirthDate = model.BirthDate.Date;
            personModel.CountryName = model.CountryName;
            personModel.CityName = model.CityName;
            personModel.LastName = model.LastName;
            personModel.TelephoneNumber = model.TelephoneNumber;
            #endregion

            #region Model to model - User
            userModel.Username = model.Username;
            userModel.Password = model.Password;
            userModel.Email = model.Email;
            userModel.Role = model.Role;
            #endregion

            #region Model to model - Athlete
            athleteModel.Username = model.Username;
            athleteModel.Weight = model.Weight;
            athleteModel.Height = model.Height;
            athleteModel.BikerType = athleteModel.GetBikerTypeID(model.BikerType);
            #endregion

            //Updating Values into Database
            personModel.UpdatePerson();
            var response = athleteModel.UpdateAthlete();

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);
        }

        [Route("photo")]
        [HttpGet]
        public IHttpActionResult GetPhoto(string iduser)
        {
            Person personModel = new Person(iduser);
            var response = personModel.GetPhoto();

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);

        }

        [Route("photo")]
        [HttpPost]
        public IHttpActionResult PostPhoto(AthleteUser model)
        {
            Person personModel = new Person();
            personModel.Photo = model.Photo;
            personModel.Email = model.Email;

            var response = personModel.UpdatePhoto(model.Photo);

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);

        }
    }

  
}
