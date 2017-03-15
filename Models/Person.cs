using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Speedforce_DataAccess;
using API_Speedforce.Business;

namespace API_Speedforce.Models
{
    public class Person
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
        public TB_Personas PersonEntity { get; set; }

        public Person() { }

        public Person(string email)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine("---DB/Entity Problem---");
                Console.WriteLine();
                Console.WriteLine(ex);
            }

            
        }

       public void UpdatePerson()
        {
            PersonEntity = new TB_Personas();

            try
            {
                PersonEntity.Nombre = this.Name;
                PersonEntity.Apellidos = this.LastName;
                PersonEntity.Email = this.Email;
                PersonEntity.FechaNacimiento = this.DateBirth.Date;
                PersonEntity.ID_Sexo = this.Sex;
                PersonEntity.NombreCiudad = this.CityName;
                PersonEntity.NombrePais = this.CountryName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        } 

        public OperationResponse<Person> AddPerson()
        {
            var result = new OperationResponse<Person>();
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    if (!entities.TB_Personas.Any(cred => cred.Email == this.Email))
                    {
                        UpdatePerson();
                        entities.Entry(PersonEntity).State = System.Data.Entity.EntityState.Added;
                        entities.SaveChanges();
                        return result.Complete(this);
                    }
                    else
                        return result.Failed("Email ya está siendo usado.");

                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }

        }
    }
}