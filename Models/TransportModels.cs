using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// File of classes created to take all data from json requests through their properties.
/// </summary>
namespace API_Speedforce.Models
{
    public class AthleteUser
    {
        public String Username { get; set; }
        public String Password { get; set; }
        public String Role { get; set; }
        public String Email { get; set; }
        public String Name { get; set; }
        public String LastName { get; set; }
        public String Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public String CityName { get; set; }
        public String CountryName { get; set; }
        public String TelephoneNumber { get; set; }
        public Double Weight { get; set; }
        public Double Height { get; set; }
        public String BikerType { get; set; }
        public Int32 Bike { get; set; }
        public String Photo { get; set; }
    }

    public class TrainerUser
    {
        public String Username { get; set; }
        public String Password { get; set; }
        public String Role { get; set; }
        public String Email { get; set; }
        public String Name { get; set; }
        public String LastName { get; set; }
        public String Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public String CityName { get; set; }
        public String CountryName { get; set; }
        public String TelephoneNumber { get; set; }
        public Boolean Certified { get; set; }
    }

    public class TrainingSessionModel
    {
        public String SessionID { get; set; }
        public String UserID { get; set; }
        public Double AverageBPM { get; set; }
        public Double BurntCalories { get; set; }
        public String RouteID { get; set; }
        public String RouteName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Double Distance { get; set; }
        public Double RelativeHumidity { get; set; }
        public Double Temperature { get; set; }
        public String TrainingTypeID { get; set; }
        public String SessionStatusID { get; set; }
        public String ClimateConditionID { get; set; }
        public List<LocationModel> Coordinates { get; set; }
        public String CountryName { get; set; }
        public String CityName { get; set; }
        public  TimeSpan Duration { get; set; }
    }

    public class LocationModel
    {
        public Double lat { get; set; }
        public Double lng { get; set; }
        public Boolean isMilestone { get; set; }
    }

    public class TrainerAthleteModel
    {
        public String AthleteID { get; set; }
        public String TrainerID { get; set; }
    }

    public class ValidationModel
    {
        public LocationModel StartPoint { get; set; }
        public LocationModel EndPoint { get; set; }
        public Double Distance { get; set; }
    }

    public class LoginModel
    {
        public String Username { get; set; }
        public String Password { get; set; }
    }

    public class RouteModel
    {
        public String RouteID { get; set; }
        public String RouteName { get; set; }
        public List<LocationModel> Coordinates { get; set; }
    }

    public class BIEntryModel
    {
        public String UserID { get; set; }
        public String RouteID { get; set; }
    }

}