using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_Speedforce.Models;
using System.Text;
using System.Threading.Tasks;

namespace API_Speedforce.Controllers
{
    /// <summary>
    /// Controller that handles all requests related to Training Sessions.
    /// </summary>
    [RoutePrefix("api/training")]
    public class TrainingSessionController : ApiController
    {
        /// <summary>
        /// Function that lists all registered Training Session from a certain user.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>A list of all the training sessions.</returns>
        [Route("sessionlist/{userid}")]
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
        /// Function that sends a registered Training Session from a certain user.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>A training sessions.</returns>
        [Route("session/{sessionid}")]
        [HttpGet]
        public IHttpActionResult GetSession(string sessionid)
        {
            TrainingSession model = new TrainingSession(sessionid);
            var response = Utility.SessionToTransport(model);
                

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);
        }

        [Route("routes")]
        [HttpPost]
        public IHttpActionResult CheckRoutes(ValidationModel model)
        {
            List<RouteModel> routeModel = new List<RouteModel>();

           var response = Utility.FilteredRoutes(Utility.isRouteSimilar(model), model.Distance);

            foreach (var item in response.Body)
            {
                RouteModel r = new RouteModel();

                r.RouteID = item.ID_Route;
                r.RouteName = item.RouteName;
                foreach(var item2 in item.LocationList)
                {
                    LocationModel loc = new LocationModel();

                    loc.lat = item2.Latitude;
                    loc.lng = item2.Longitude;
                    loc.isMilestone = item2.isMilestone;

                    r.Coordinates.Add(loc);
                    
                }
            }

            if (response.IsComplete())
                return Ok(routeModel);
            else
                return BadRequest(response.Message);
        }

        /// <summary>
        /// Function to register a new Training Session. 
        /// </summary>
        /// <param name="model">Model represents all the attributes expected to receive as a json in the body of the request.</param>
        /// <returns></returns>
        [Route("logsession")]
        [HttpPost]
        public IHttpActionResult LogSession(TrainingSessionModel model)
        {
            TrainingSession sessionModel = new TrainingSession();
            Route routeModel = new Route();
            Location locationModel;

            #region From Json to Model (Route)
            routeModel.CityID = Utility.GetCityID(model.CityName, Utility.GetCountryID(model.CountryName));
            routeModel.ID_Route = model.RouteID;
            routeModel.RouteName = model.RouteName;
            routeModel.FillLocationList(model);
            #endregion

            #region From Json to Model (Training Session)
            sessionModel.AverageBPM = model.AverageBPM;
            sessionModel.BurntCalories = model.BurntCalories;
            sessionModel.ClimateConditionID = Utility.GetClimateConditionID(model.ClimateConditionID);
            sessionModel.Distance = model.Distance;
            sessionModel.EndTime = model.EndTime;
            sessionModel.RelativeHumidity = model.RelativeHumidity;
            sessionModel.RouteID = model.RouteID;
            sessionModel.SessionID = model.SessionID;
            sessionModel.SessionStatusID = Utility.GetSessionStatusID(model.SessionStatusID);
            sessionModel.StartTime = model.StartTime;
            sessionModel.Temperature = model.Temperature;
            sessionModel.TrainingTypeID = Utility.GetTrainingTypeID(model.TrainingTypeID);
            sessionModel.UserID = model.UserID;
            #endregion

            
            routeModel.AddRoute();
            foreach (var item in routeModel.LocationList)
            {
                locationModel = new Location();
                locationModel.ID_Route = item.ID_Route;
                locationModel.isEndPoint = item.isEndPoint;
                locationModel.isStartPoint = item.isStartPoint;
                locationModel.isMilestone = item.isMilestone;
                locationModel.Latitude = item.Latitude;
                locationModel.Longitude = item.Longitude;

                locationModel.AddLocation();
            }
            
            
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

            var response = Utility.SessionToTransport(sessionModel.FindSavedSession(userid));

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);


        }

        /// <summary>
        /// Function to save a pending Training Session.
        /// </summary>
        /// <returns></returns>
        [Route("postsession")]
        [HttpPost]
        public IHttpActionResult CreateSession(BIEntryModel model)
        {
            TrainingSession sessionModel = new TrainingSession();

            var response = sessionModel.AddSession(model);

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);


        }

        /// <summary>
        /// Function to send a saved Training Session to the athlete.
        /// </summary>
        /// <param name="userid">The unique user id of the Trainer</param>
        /// <remarks>It is expected the user id it's the senders ID</remarks>
        [Route("athletelist/{userid}")]
        [HttpGet]
        public IHttpActionResult GetAthleteList(string userid)
        {

            var response = Utility.GetAthletes(userid);

            if (response.IsComplete())
                return Ok(response.Body);
            else
                return BadRequest(response.Message);


        }

    }
}
