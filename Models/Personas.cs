using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Speedforce_DataAccess;

namespace API_Speedforce.Models
{
    public class Personas
    {
        public String Email { get; set; }
        public Int32 Sex { get; set; }
        public String Name { get; set; }
        public String LastName { get; set; }
        public DateTime DateBirth { get; set; }
        public String CityName { get; set; }
        public String CountryName { get; set; }
        public String TelephoneNumber { get; set; }
        //public String Photo { get; set; }

        public Personas() { }

        public Personas(string email)
        {
            using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
            {
                Email = entities.TB_Personas.Find(email).Email;
                Name = entities.TB_Personas.Find(email).Nombre;
                LastName = entities.TB_Personas.Find(email).Apellidos;
                Sex = entities.TB_Personas.Find(email).ID_Sexo;
                CityName = entities.TB_Personas.Find(email).NombreCiudad;
                CountryName = entities.TB_Personas.Find(email).NombrePais;
                DateBirth = entities.TB_Personas.Find(email).FechaNacimiento;
                TelephoneNumber = entities.TB_Personas.Find(email).NumeroTelefono;

            }
        }
    }
}