using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_Speedforce.Models;

namespace API_Speedforce.Controllers
{
    /// <summary>
    /// Controller that handles all requests related to Training Sessions.
    /// </summary>
    [RoutePrefix("api/speedforce/training")]
    public class TrainingSessionController : ApiController
    {
        /// <summary>
        /// Function that lists all registered Training Session from a certain user.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>A list of all the training sessions.</returns>
        [Route("list/{userid}")]
        [HttpGet]
        public IHttpActionResult SessionList(string userid)
        {
            TrainingSession model = new TrainingSession();
            var response = model.GetTrainingSessions(userid);

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Function to register a new Training Session. 
        /// </summary>
        /// <param name="model">Model represents all the attributes expected to receive as a json in the body of the request.</param>
        /// <returns></returns>
        [Route("log")]
        [HttpPost]
        public IHttpActionResult LogSession(TrainingSessionModel model)
        {
            TrainingSession sessionModel = new TrainingSession();

            #region From a Model to another
            sessionModel.AverageBPM = model.AverageBPM;
            sessionModel.BurntCalories = model.BurntCalories;
            sessionModel.ClimateConditionID = sessionModel.GetClimateConditionID(model.ClimateConditionID);
            sessionModel.Distance = model.Distance;
            sessionModel.EndTime = model.EndTime;
            sessionModel.RelativeHumidity = model.RelativeHumidity;
            sessionModel.RouteID = model.RouteID;
            sessionModel.SessionID = model.SessionID;
            sessionModel.SessionStatusID = sessionModel.GetSessionStatusID(model.SessionStatusID);
            sessionModel.StartTime = model.StartTime;
            sessionModel.Temperature = model.Temperature;
            sessionModel.TrainingTypeID = sessionModel.GetTrainingTypeID(model.TrainingTypeID);
            sessionModel.UserID = model.UserID;
            #endregion

            var response = sessionModel.AddSession();

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);
        }


        /// <summary>
        /// Function to send a saved Training Session to the athlete.
        /// </summary>
        /// <param name="userid">The unique user id</param>
        /// <remarks>It is expected the user id it's the senders ID</remarks>
        /// <returns></returns>
        [Route("challenge/{userid}")]
        [HttpGet]
        public IHttpActionResult GetChallenge(string userid)
        {
            TrainingSession sessionModel = new TrainingSession();

            var response = sessionModel.FindSavedSession(userid);

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);


        }
    }
}
