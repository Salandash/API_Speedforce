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
        public Int32 Role { get; set; }
        public String Email { get; set; }
        public String Name { get; set; }
        public String LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public String CityName { get; set; }
        public String CountryName { get; set; }
        public String TelephoneNumber { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public String BikerType { get; set; }
        public String Sex { get; set; }
        public String Photo { get; set; }
    }

    public class TrainingSessionModel
    {
        public String SessionID { get; set; }
        public String UserID { get; set; }
        public float AverageBPM { get; set; }
        public float BurntCalories { get; set; }
        public String RouteID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public float Distance { get; set; }
        public float RelativeHumidity { get; set; }
        public float Temperature { get; set; }
        public string TrainingTypeID { get; set; }
        public string SessionStatusID { get; set; }
        public string ClimateConditionID { get; set; }
    }
}