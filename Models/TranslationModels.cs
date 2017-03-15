using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Speedforce.Models
{
    public class UserModel
    {
        public String Username { get; set; }
        public String Password { get; set; }
        public Int32 Role { get; set; }
        public String Email { get; set; }
        public String Name { get; set; }
        public String LastName { get; set; }
        public DateTime DateBirth { get; set; }
        public String CityName { get; set; }
        public String CountryName { get; set; }
        public String TelephoneNumber { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public Int32 Bicicleta { get; set; }
        public Int32 TipoCiclista { get; set; }
        public Int32 Sex { get; set; }
    }
}