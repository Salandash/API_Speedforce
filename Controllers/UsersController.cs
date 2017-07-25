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
    [RoutePrefix("api/users")]
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
        public IHttpActionResult AthleteLogin (LoginModel model)
        {
            User userModel = new User();
            try
            {
                userModel.Username = model.Username;
                userModel.Password = model.Password;
            }
            catch (Exception ex)
            {
                return BadRequest("Información Incorrecta");
            }

            var response = userModel.VerifyLogin();
            AthleteUser athleteModel = new AthleteUser();
            

            #region User Info Into Athlete Model
            athleteModel.Username = userModel.Username;
            athleteModel.Password = userModel.Password;
            #endregion

            athleteModel.Email = Utility.GetEmail(userModel.Username);

            Person personModel = new Person(athleteModel.Email);
            #region Person Info into Athlete Model
            
            athleteModel.Name = personModel.Name;
            athleteModel.LastName = personModel.LastName;
            athleteModel.Sex = Utility.GetSexString(personModel.Sex);
            athleteModel.BirthDate = personModel.BirthDate;
            athleteModel.CityName = Utility.GetCityString(personModel.CityID);
            athleteModel.CountryName = Utility.GetCountryString(personModel.CityID);
            athleteModel.TelephoneNumber = personModel.TelephoneNumber;
            #endregion

            Athlete athleteObj = new Athlete(athleteModel.Username);
            #region Athlete Info into Athlete Model
            athleteModel.Weight = athleteObj.Weight;
            athleteModel.Height = athleteObj.Height;
            athleteModel.BikerType = Utility.GetBikerTypeString(athleteObj.BikerType);
            athleteModel.Bike = athleteObj.Bike;
            #endregion

            if (response.IsComplete())
                return Ok(athleteModel);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Function to verify a trainer logs in into the system.
        /// </summary>
        /// <param name="model">All the atlhlete's attributes from a json file</param>
        /// <returns></returns>
        [Route("loginT")]
        [HttpPost]
        public IHttpActionResult TrainerLogin(User model)
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
            personModel.Sex = Utility.GetSexID(model.Sex);
            personModel.BirthDate = model.BirthDate;
            personModel.CityID = Utility.GetCityID(model.CityName, Utility.GetCountryID(model.CountryName));
            personModel.LastName = model.LastName;
            personModel.TelephoneNumber = model.TelephoneNumber;
            #endregion

            if(model.Role != "Atleta")
                return BadRequest("Esta función es para atletas.");

            #region Model to model - User
            userModel.Username = model.Username;
            userModel.Password = model.Password;
            userModel.Email = model.Email;
            userModel.Role = Utility.GetRoleID(model.Role);
            #endregion

            #region Model to model - Athlete
            athleteModel.Username = model.Username;
            athleteModel.Weight = model.Weight;
            athleteModel.Height = model.Height;
            athleteModel.BikerType = Utility.GetBikerTypeID(model.BikerType);
            athleteModel.Bike = model.Bike;
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
        /// Function to register a new athlete to the system.
        /// </summary>
        /// <param name="model">All the atlhlete's attributes from a json file</param>
        /// <returns></returns>
        [Route("registerT")]
        [HttpPost]
        public IHttpActionResult RegisterTrainer(TrainerUser model)
        {
            User userModel = new User();
            Person personModel = new Person();
            Trainer trainerModel = new Trainer();

            #region Model to model - Person
            personModel.Email = model.Email;
            personModel.Name = model.Name;
            personModel.Sex = Utility.GetSexID(model.Sex);
            personModel.BirthDate = model.BirthDate;
            personModel.CityID = Utility.GetCityID(model.CityName, Utility.GetCountryID(model.CountryName));
            personModel.LastName = model.LastName;
            personModel.TelephoneNumber = model.TelephoneNumber;
            #endregion

            if (model.Role != "Entrenador")
                return BadRequest("Esta función es para entrenadores.");

            #region Model to model - User
            userModel.Username = model.Username;
            userModel.Password = model.Password;
            userModel.Email = model.Email;
            userModel.Role = Utility.GetRoleID(model.Role);
            #endregion

            #region Model to model - Trainer
            trainerModel.Username = model.Username;
            trainerModel.Certified = true;
            #endregion

            //Adding values to Databases
            personModel.AddPerson();
            var response = userModel.AddUser();
            trainerModel.AddTrainer();

            if (response.IsComplete())
                return Ok("Entrenador Agregado.");
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
        public IHttpActionResult GetPersonInfo(string email)
        {
            Person personModel = new Person(email);

            var response = personModel.GetPerson(email);

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Function to pair up an Athlete with a Trainer
        /// </summary>
        /// <param name="athleteid">Unique ID of the athlete</param>
        /// /// <param name="trainerid">Unique ID of the trainer</param>
        [Route("trainer/athlete")]
        [HttpPost]
        public IHttpActionResult PairAthlete(TrainerAthleteModel model)
        {
            TrainerAthlete pairModel = new TrainerAthlete();

            pairModel.AthleteID = model.AthleteID;
            pairModel.TrainerID = model.TrainerID;

            var response = pairModel.AddPair();

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);
        }

        [Route("removeA")]
        [HttpPost]
        public IHttpActionResult UnpairAthlete(TrainerAthleteModel model)
        {
            TrainerAthlete pairModel = new TrainerAthlete();

            pairModel.AthleteID = model.AthleteID;
            pairModel.TrainerID = model.TrainerID;

            var response = pairModel.DeletePair();

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);
        }


        [Route("athleteList/{trainerid}")]
        [HttpGet]
        public IHttpActionResult GetAthletes(string trainerid)
        {
            var response = Utility.GetAthletes(trainerid);

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
            personModel.Sex = Utility.GetSexID(model.Sex);
            personModel.BirthDate = model.BirthDate.Date;
            personModel.CityID = Utility.GetCityID(model.CityName, Utility.GetCountryID(model.CountryName));
            personModel.LastName = model.LastName;
            personModel.TelephoneNumber = model.TelephoneNumber;
            #endregion

            #region Model to model - User
            userModel.Username = model.Username;
            userModel.Password = model.Password;
            userModel.Email = model.Email;
            userModel.Role = Utility.GetRoleID(model.Role);
            #endregion

            #region Model to model - Athlete
            athleteModel.Username = model.Username;
            athleteModel.Weight = model.Weight;
            athleteModel.Height = model.Height;
            athleteModel.BikerType = Utility.GetBikerTypeID(model.BikerType);
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
